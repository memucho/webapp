using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TrueOrFalse.Web;

public class ToolsModel : BaseModel
{
    public UIMessage Message;

    public int CategoryToAddId { get; set; }

    public int CategoryToRemoveId { get; set; }
    public new int UserId { get; set; }
    public  PaymentStatus PaymentStatus { get; set; }

}
