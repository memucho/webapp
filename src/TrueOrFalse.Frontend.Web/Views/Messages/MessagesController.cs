﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse;

public class MessagesController : BaseController
{
    [SetMenu(MenuEntry.Messages)]
    public ActionResult Messages()
    {
        if (!_sessionUser.IsLoggedIn)
            return View(new MessageModel(new List<Message>()
            {
                new Message
                {
                    Subject = "<p>Hallo Unbekannter!</p>",
                    Body = @"<p>
                                schön das Du bei uns vorbeischaust. 
                                Eigentlich gibt es hier nichts zu sehen, 
                                wenn Du nicht angemeldet ist.
                            </p>
                            <p>Wir wünschen Dir weiter viel Spass beim Stöbern.</p>
                            <p>
                                Viele Grüße<br>
                                Jule & Robert
                            </p>",
                    DateCreated = DateTime.Now,
                    IsRead = false
                }
            })
            {
                IsLoggedIn = false
            });

        var messages = Resolve<MessageRepository>()
            .GetForUser(_sessionUser.User.Id);

        return View(new MessageModel(messages));
    }
}