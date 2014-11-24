<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:param name="rootRelPath"/>

	<xsl:template match="/Topic">
		<html>
			<head>
				<title>
					<xsl:value-of select="@Title"/>
				</title>
				<link rel="stylesheet" type="text/css">
					<xsl:attribute name="href"><xsl:value-of select="$rootRelPath"/>style.css</xsl:attribute>
				</link>
			</head>
			<body>
				<div class="CommonContent">
					<div class="CommonContentArea">
						<h1>
							<xsl:value-of select="@Title"/>
						</h1>
						<xsl:value-of disable-output-escaping="yes" select="RenderedBody"/>
					</div>
				</div>
				<div id="footer">
					This revision (<xsl:value-of select="@Revision"/>) was last Modified <xsl:value-of select="@Modified"/> by <xsl:value-of select="@UserName"/>.
				</div>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>