﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrueOrFalse.Frontend.Web.Models 
{
    public class RegisterModel : ModelBase
    {
        [Required(ErrorMessage="Wir benötigen Deinen Benutzernamen.")]
        
        [StringLength(20, MinimumLength=3, ErrorMessage = "Mind. 3 Zeichen kurz und maximal 20 Zeichen lang sein.")]
        [DisplayName("Benutzername")]
        //[IsNew(ErrorMessage = "Someone has already signed up with this e-mail address.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Wir benötigen Deine E-Mail Adresse.")]
        [RegularExpression(EmailRegEx, ErrorMessage = "Wir benötigen eine gültige E-Mail Adresse.")]
        [DisplayName("Email")]  
        public string Email { get; set; }

        [Required(ErrorMessage ="Ein Passwort ist Pflicht!")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Mind. 5 Zeichen kurz und maximal 20 Zeichen lang sein.")]
        [DisplayName("Password")]

        public string Password { get; set; }

        [DisplayName("AGB Bestätigen: [TODO erstellen und verlinken]")]
        public bool TermsAndConditionsApproved { get; set; }

        public RegisterModel()
        {
            ShowLeftMenu = false;
        }

        private const string EmailRegEx = @"^(([^<>()[\]\\.,;:\s@\""]+"
        + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
        + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
        + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
        + @"[a-zA-Z]{2,}))$";

    }


}