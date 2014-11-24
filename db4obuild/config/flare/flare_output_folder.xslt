<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:param name="flareOutputPath"/>

	<xsl:template match="@OutputFolder[parent::CatapultTarget]">
		<xsl:attribute name="OutputFolder">
			<xsl:value-of select="$flareOutputPath"/>
		</xsl:attribute>
	</xsl:template>

	<xsl:template match="@*|node()">
		<xsl:copy>
			<xsl:apply-templates select="@*|node()"/>
		</xsl:copy>
	</xsl:template>
</xsl:stylesheet>



