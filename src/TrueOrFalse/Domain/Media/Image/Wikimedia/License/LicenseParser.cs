using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using EasyNetQ.Events;
using FluentNHibernate.Utils;
using NHibernate.Criterion;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Mapping;
using SolrNet.Utils;
using TrueOrFalse;
using TrueOrFalse.WikiMarkup;

public class LicenseParser
{
    public static List<License> GetAllParsedLicenses(string wikiMarkup)
    {
        return LicenseRepository.GetAllRegisteredLicenses()
            .Where(license => ParseTemplate.TokenizeMarkup(wikiMarkup).Any(x => !String.IsNullOrEmpty(license.WikiSearchString)
                                                        && x.ToLower() == license.WikiSearchString.ToLower()))
            .ToList();
    }

    public static List<License> GetAuthorizedParsedLicenses(string wikiMarkup)
    {
         return LicenseRepository.GetAllAuthorizedLicenses()
            .Where(license => ParseTemplate.TokenizeMarkup(wikiMarkup).Any(x => !String.IsNullOrEmpty(license.WikiSearchString) 
                                                        && x.ToLower() == license.WikiSearchString.ToLower()))
            .ToList();
    }

    public static List<License> GetAuthorizedParsedLicenses(List<License> allLicenses)
    {
        return SortLicenses(LicenseRepository.GetAllAuthorizedLicenses()
                                .Where(license => allLicenses.Any(x => x.Id == license.Id))
                                .ToList());
    }

    public static IList<License> GetNonAuthorizedParsedLicenses(string wikiMarkup)
    {
        return GetAllParsedLicenses(wikiMarkup).Except(GetAuthorizedParsedLicenses(wikiMarkup)).ToList();
    }

    public static License SuggestMainLicenseFromMarkup(ImageMetaData imageMetaData)
    {
        return GetAuthorizedParsedLicenses(imageMetaData.Markup)
                .Where(license => CheckLicenseRequirementsWithMarkup(license, imageMetaData).AllRequirementsMet)
                .ToList()
                .FirstOrDefault();
    }

    public static License SuggestMainLicenseFromParsedList(ImageMetaData imageMetaData)
    {
        return GetAuthorizedParsedLicenses(License.FromLicenseIdList(imageMetaData.AllRegisteredLicenses))
            .Where(license => CheckLicenseRequirementsWithDb(license, imageMetaData).AllRequirementsMet)
                .ToList()
                .FirstOrDefault();
    }

    public static List<License> ParseAllRegisteredLicenses(string markup)
    {
        return SortLicenses(GetAllParsedLicenses(markup));
    } 

    public static List<License> SortLicenses(List<License> licenseList)
    {
        return licenseList
            .OrderBy(license => license.LicenseRequirementsType.GetRank())
            .ThenByDescending(license => new GetCcLicenseComponents(license).CcVersion)
            .ThenBy(PriotizeByCcJurisdictionToken)
            .ThenBy(license => String.IsNullOrEmpty(license.WikiSearchString))//To have empty strings/null at the end
            .ThenBy(license => license.WikiSearchString)
            .ToList();
    }

    public static LicenseNotifications CheckLicenseRequirementsWithMarkup(License license, ImageMetaData imageMetaData)
    {
        return CheckLicenseRequirements(
            license,
            imageMetaData,
            !String.IsNullOrEmpty(ParseImageMarkup.Run(imageMetaData.Markup).AuthorName)
                ? ParseImageMarkup.Run(imageMetaData.Markup).AuthorName
                : "");
    }

    public static LicenseNotifications CheckLicenseRequirementsWithDb(License license, ImageMetaData imageMetaData)
    {
        return CheckLicenseRequirements(
            license,
            imageMetaData,
            imageMetaData.AuthorParsed);
    }

    private static LicenseNotifications CheckLicenseRequirements(License license, ImageMetaData imageMetaData, string author)
    {
        var licenseNotifications = new LicenseNotifications();
        if (license.AuthorRequired.IsTrue() &&
            String.IsNullOrEmpty(author) &&
            String.IsNullOrEmpty(imageMetaData.ManualEntriesFromJson().AuthorManuallyAdded))
        {
            licenseNotifications.AuthorIsMissing = true;
            licenseNotifications.AllRequirementsMet = false;
        }
        if (license.LicenseLinkRequired.IsTrue() && String.IsNullOrEmpty(license.LicenseLink))
        {
            licenseNotifications.LicenseLinkIsMissing = true;
            licenseNotifications.AllRequirementsMet = false;
        }
        if (license.CopyOfLicenseTextRequired.IsTrue() && String.IsNullOrEmpty(license.CopyOfLicenseTextUrl))
        {
            licenseNotifications.LocalCopyOfLicenseUrlMissing = true;
            licenseNotifications.AllRequirementsMet = false;
        }
        return licenseNotifications;
    }

    public static LicenseState CheckImageLicenseState(License license, ImageMetaData imageMetaData)
    {
        if (LicenseRepository.GetAllRegisteredLicenses().Any(l => l.Id == license.Id))
        {
            if (LicenseRepository.GetAllAuthorizedLicenses().Any(l => l.Id == license.Id))
            {
                return CheckLicenseRequirementsWithDb(license, imageMetaData).AllRequirementsMet ? LicenseState.IsApplicableForImage : LicenseState.AuthorizedButInfoMissing;
            }
            return LicenseState.IsNotAuthorized;
        }
        return LicenseState.NotSpecified;
    }

    public static int PriotizeByCcJurisdictionToken(License license)    
    {
        var licenseComponents = new GetCcLicenseComponents(license);

        if (licenseComponents.CcJurisdictionPortsToken == "")
            return 1;
        if (licenseComponents.CcJurisdictionPortsToken == "de")
            return 2;
        return 99;
    }

    public static List<string> LicenseRegexSearchExpressions()
    {
        //$todo: refine
        return new List<string> { "^cc-", "^pd-", "^gfdl" };
    }

    public static List<string> GetOtherPossibleLicenseStrings(string wikiMarkup)
    {
        return ParseTemplate.TokenizeMarkup(wikiMarkup)
            .Where(token => LicenseRegexSearchExpressions()
            .Any(expression => Regex.Match(token, expression, RegexOptions.IgnoreCase).Success))
            .Except(GetAllParsedLicenses(wikiMarkup).Select(license => license.WikiSearchString))
            .ToList();
    }

    public static string GetWikiDetailsPageFromSourceUrl(string sourceUrl)
    {
        return !String.IsNullOrEmpty(sourceUrl) && sourceUrl.StartsWith("http://upload.wikimedia.org")
            ? "http://commons.wikimedia.org/wiki/File:" + Regex.Split(sourceUrl, "/").Last()
            : "";
    }
}


