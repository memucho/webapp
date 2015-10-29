﻿using System;
using System.Collections.Generic;
using Seedworks.Lib;

public class CurvesJsonCmd
{
    public int IntervalCount { get; set; }
    public string Interval { get; set; }
    public List<CurveDesc> Curves { get; set; }

    public CurvesJsonCmd()
    {
        Curves = new List<CurveDesc>();
    }

    public void Process()
    {
        var answerFeatureRepo = Sl.R<AnswerFeatureRepo>();

        foreach (var curve in Curves)
        {
            if (curve.AnswerFeatureId.IsNumeric())
            {
                curve.AnswerFeature = answerFeatureRepo.GetById(curve.AnswerFeatureId.ToInt32());
            }
        }
    }
}

public class CurveDesc
{
    public bool Show { get; set; }
    public string AnswerFeatureId { get; set; }
    public AnswerFeature AnswerFeature;

    /// <summary>
    /// Question category, possible values: 
    /// - nobrainer, 
    /// - easy, 
    /// - middle
    /// - hard
    /// </summary>
    public string QuestionFeatureId { get; set; }
    public QuestionFeature QuestionFeature;

    public string ColumnId { get { return ColumnLabel.Replace(" ", ""); } }

    public string ColumnLabel
    {
        get
        {
            if (AnswerFeature == null && QuestionFeature == null)
                return "Alle";

            var result = "";

            if (AnswerFeature != null && String.IsNullOrEmpty(AnswerFeature.Name))
                result = AnswerFeature.Name;

            if (QuestionFeature != null && String.IsNullOrEmpty(QuestionFeature.Name))
                result += " " + QuestionFeature.Name;

            return result;
        }
    }

    public ForgettingCurve LoadForgettingCurve(ForgettingCurveInterval interval, int maxIntervalCount)
    {
        if (AnswerFeature != null)
            return ForgettingCurveLoader.GetForFeature(AnswerFeature, interval, maxIntervalCount);

        return ForgettingCurveLoader.GetForAll(interval, maxIntervalCount);
    }
}