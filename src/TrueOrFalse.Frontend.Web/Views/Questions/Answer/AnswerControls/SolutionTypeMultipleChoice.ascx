<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMultipleChoice>" %>

<% var localChoices = Model.Choices;
   if (!Model.IsSolutionOrdered)
   {
       var random = new Random();
       localChoices = Model.Choices.OrderBy(x => random.Next()).ToList();
   }

   foreach (var choice in localChoices)
    { %>
    <div class="checkbox">
        <label>
            <input type="checkbox" name="answer" value="<%: choice.Text %>" /> <%: choice.Text %> <br />
        </label>
    </div>
<% } %>
<br/>
<h6 class = "ItemInfo">
    Es können keine oder mehrere Antworten richtig sein!
</h6>