<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:user="http://mycompany.com/mynamespace">

	<msxsl:script language="JScript" implements-prefix="user">
		function toLower(s){
			return s.toLowerCase();
		}
	</msxsl:script>
	
	<xsl:template match="/">
		<html>
			<head>
				<title>

				</title>
				
<script type="text/javascript" src="simpletreemenu.js">
/***********************************************
* Simple Tree Menu- &amp;copy; Dynamic Drive DHTML code library (www.dynamicdrive.com)
* This notice MUST stay intact for legal use
* Visit Dynamic Drive at http://www.dynamicdrive.com/ for full source code
***********************************************/
</script>
<link rel="stylesheet" type="text/css" href="simpletree.css" />

				<link rel="stylesheet" type="text/css" href="style.css" />
				<style>
body{
background-color:#838383;
}
				</style>
				<base target="content" />
			</head>
			<body>
				<a href="http://www.db4o.com" target="_blank"><img src="db4objects1.jpg" border="0" /></a>

				<div>
					<BR/>
					<input type="button" onClick="ddtreemenu.flatten('refTree', 'expand')" value="Expand All" style="font-size: 80%"/> | 
					<input type="button" onClick="ddtreemenu.flatten('refTree', 'contact')" value="Collapse All" style="font-size: 80%"/>
				</div>

				<div id="NavFrameTOC">
					<ul id="refTree" class="treeview">
						<xsl:apply-templates select="/Topic">
							<xsl:sort data-type="number"  select="@TopicWeight"/>
							<xsl:sort select="@Title"/>
						</xsl:apply-templates>
					</ul>
				</div>
				<xsl:if test="/Topic">
					<script lang="javascript">
						ddtreemenu.openfolder = "images/open.gif";
						ddtreemenu.closefolder = "images/closed.gif";
						//ddtreemenu.createTree(treeid, enablepersist, opt_persist_in_days (default is 1));
						ddtreemenu.createTree("refTree", false, 5); //false -> do not persist state
						//ddtreemenu.flatten('refTree', 'expand');
						parent['content'].location.href = '<xsl:value-of select="/Topic/@RelPath"/>.html';
					</script>
				</xsl:if>
			</body>
		</html>
	</xsl:template>

	<xsl:template match="Topic[@ShowInNavigation = 'True']">
		<li><a target="content">
				<xsl:attribute name="href"><xsl:value-of select="@RelPath"/>.html</xsl:attribute>
				<xsl:value-of select="@Title"/>
			</a>
			<xsl:if test="Topic">
				<ul>
					<xsl:apply-templates select="Topic">
						<xsl:sort data-type="number" select="@TopicWeight"/>
						<xsl:sort select="@Title"/>
					</xsl:apply-templates>
				</ul>
			</xsl:if>
		</li>
	</xsl:template>
</xsl:stylesheet>