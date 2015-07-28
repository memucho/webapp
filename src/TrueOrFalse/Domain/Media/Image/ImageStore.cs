﻿using System;
using TrueOrFalse;

public class ImageStore : IRegisterAsInstancePerLifetime
{
    private readonly WikiImageMetaLoader _metaLoader;
    private readonly ImageMetaDataRepository _imgMetaRepo;

    public ImageStore(
        WikiImageMetaLoader metaLoader,
        ImageMetaDataRepository imgMetaRepo)
    {
        _metaLoader = metaLoader;
        _imgMetaRepo = imgMetaRepo;
    }

    public void RunWikimedia<T>(
        string imageWikiFileName, 
        int typeId, 
        ImageType imageType,
        int userId) where T : IImageSettings
    {
        var wikiMetaData = _metaLoader.Run(imageWikiFileName, 1024);

        var imageSettings = Activator.CreateInstance<T>();
        imageSettings.Init(typeId);
        imageSettings.DeleteFiles(); //old files..

        using (var stream = wikiMetaData.GetThumbImageStream()){
            //$temp: Bildbreite uebergeben und abhaengig davon versch. Groessen speichern?
            SaveImageToFile.Run(stream, imageSettings); 
        }

        _imgMetaRepo.StoreWiki(typeId, imageType, userId, wikiMetaData);
    }

    public void RunUploaded<T>(TmpImage tmpImage, int typeId, int userId, string licenseGiverName) where T : IImageSettings
    {
        var imageSettings = Activator.CreateInstance<T>();
        imageSettings.Init(typeId);
        imageSettings.DeleteFiles(); //old files..

        using (var stream = tmpImage.GetStream()){
            SaveImageToFile.Run(stream, imageSettings);
        }

        _imgMetaRepo.StoreUploaded(typeId, userId, imageSettings.ImageType, licenseGiverName);
    }
}