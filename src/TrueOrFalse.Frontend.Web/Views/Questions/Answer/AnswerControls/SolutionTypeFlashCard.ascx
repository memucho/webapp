﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionFlashCard>" %>
<%@ Import Namespace="TrueOrFalse.Web" %>


<div id="flashCardContent">
    <div class="front flashCardContentSite" id="flashCard-front"></div>
    <div class="back flashCardContentSite" id="flashCard-back">
            <%= MarkdownInit.Run().Transform(Model.Text) %>
    </div>
</div>
<script type="text/javascript">
    $('#flashCardContent').flip();
</script>