﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionFlashCard>" %>
<%@ Import Namespace="TrueOrFalse.Web" %>


<div id="flashCardContent">
    <div class="front" id="flashCard-front"></div>
    <div class="back" id="flashCard-back">
            <%= MarkdownInit.Run().Transform(Model.Text) %>
    </div>
</div>
<script>
    //$('#flashCardContent').flip();
</script>