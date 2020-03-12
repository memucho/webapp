using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Infrastructure;
using TrueOrFalse.Web;

public class CMSModel : BaseModel
{
    public UIMessage Message;

    public string SuggestedGames { get; set; }

    public CMSModel Init()
    {
        ConsolidateGames();
        return this;
    }

    public void ConsolidateGames()
    {
        var settings = Sl.R<DbSettingsRepo>().Get();

        if (String.IsNullOrEmpty(SuggestedGames))
        {    
            settings.SuggestedGames = SuggestedGames;
            Sl.R<DbSettingsRepo>().Update(settings);
            return;
        }

        settings.SuggestedGames = SuggestedGames;
        Sl.R<DbSettingsRepo>().Update(settings);
    }
}