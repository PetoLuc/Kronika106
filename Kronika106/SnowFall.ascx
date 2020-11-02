<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SnowFall.ascx.cs" Inherits="Kronika106.SnowFall" %>

<script src="../Scripts/snowfall.min.js"></script>
<script>                                                  
            snowFall.snow(document.body, { image: "../Content/Images/scoutFlake.png", minSize: 10, maxSize: 80, flakeCount: 5, minSpeed:1, maxSpeed:2 });            
</script>
