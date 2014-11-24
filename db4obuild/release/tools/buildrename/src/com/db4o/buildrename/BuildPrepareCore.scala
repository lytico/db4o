package com.db4o.buildrename

import java.io._
import scala.xml._
import scala.collection._

object FileExtensions {
	val extensions = Set("zip", "msi")
}

sealed case class Platform(id: String, generic: String, version: Option[String]) {
	override def toString = generic + version.map(" " + _).getOrElse("")
}

object Platform {
	private val PLATFORMS = Array(
		new Platform("net", ".NET", None),
		new Platform("net2", ".NET", Some("2.0")),
		new Platform("net35", ".NET", Some("3.5")),
		new Platform("net40", ".NET", Some("4.0")),
		new Platform("java", "Java", None)
	)

	private val ID2PLATFORM = PLATFORMS.foldLeft(immutable.Map[String, Platform]()) {(m, p) => m + (p.id -> p)}
 
	def apply(id: String) = ID2PLATFORM.get(id)
}

sealed case class Product(id: String, description: String) {
	override def toString = description
}

object Product {
	private val PRODUCTS = Array(
		new Product("db4o", "db4o"),
		new Product("dRS", "db4o Replication System")
	)

	private val ID2PRODUCT = PRODUCTS.foldLeft(immutable.Map[String, Product]()) {(m, p) => m + (p.id -> p)}

	def apply(id: String) = ID2PRODUCT.get(id)
}

case class VersionedFile(sourceFile: File, product: Product, major: String, minor: String, platform: Platform, extension: String, category: Category) {
	def targetFile = new File(sourceFile.getParent, if(category.name == Category.CONTINUOUS_CATEGORY_NAME) plainName else simpleName)
	def simpleName = product.id + "-" + major + "-" + platform.id + "." + extension
	def plainName = product.id + "-" + platform.id + "." + extension
	def fullVersion = major + "." + minor
}

object VersionedFile {
  	private val NAME_PATTERN = ("""([^-]+)-((\d+\.\d+)\.(\d+\.\d+))-(.+)\.(""" + FileExtensions.extensions.mkString("|") + ")").r

   def apply(file: File, version2Category: Map[String, Category]): Option[VersionedFile] =
		file.getName match {
		  	case NAME_PATTERN(productID, full, major, minor, platformID, extension) =>
		  		for(product <- Product(productID); platform <- Platform(platformID); category <- version2Category.get(major))
		  			yield VersionedFile(file, product, major, minor, platform, extension, category)
		  	case _ => None
  		} 
}

class VersionedFileMetaRenderer(file: File, files: Iterable[VersionedFile]) {
	private val DATE = new java.text.SimpleDateFormat("yyyy-MM-dd").format(new java.util.Date) + "T00:00:00"
 
   	def render() {
		val rendered = renderFiles()
		val writer = new OutputStreamWriter(new FileOutputStream(file), "utf-8")
		try {
			writer.write(rendered)
		}
		finally {
			writer.close
		}
	}

	def renderFiles() =
		"""<?xml version="1.0" encoding="utf-8" ?>""" + "\n\n" + new PrettyPrinter(100, 2).format(metaData())
 
    def metaData(): Elem = {
      <Downloads>{files.map(metaData(_))}</Downloads>
	} 
	
 	private def metaData(file: VersionedFile): Elem =
 		<Download		
 			file={file.targetFile.getName}
 			title={file.product.description}
 			version={file.fullVersion}
 			size={file.sourceFile.length.toString}
 			releaseDate={DATE}
      		package={file.extension.toUpperCase}
 			platform={file.platform.toString}>
      		{categoryTag(file)}
      	</Download>

    private def categoryTag(file: VersionedFile): Iterable[Elem] = {
    	val categoryOption = file.category.name match {
    	  	case Category.CONTINUOUS_CATEGORY_NAME => None
    	  	case _ => Some(file.category)
    	}
    	categoryOption.map(category => <Releases>{if(category.op == ClearOp) List(<clear />) else Nil}<add name={category.name} archivePrevious={(category.op == ArchiveOp).toString} /></Releases>)
    }
}

sealed trait PreviousOp
object NoOp extends PreviousOp
object ClearOp extends PreviousOp
object ArchiveOp extends PreviousOp

case class Category(name: String, op: PreviousOp)

object Category {
	val CONTINUOUS_CATEGORY_NAME = "Continuous"
}

object BuildPrepareCore {
 	def filterFolder(folder: File, version2Category: Map[String, Category]) =
		folder.listFiles.flatMap(VersionedFile(_, version2Category))

  	def writeXMLFile(file: File, files: Iterable[VersionedFile]) =
  		new VersionedFileMetaRenderer(file, files).render()

  	def renameFile(file: VersionedFile) =
 		file.sourceFile.renameTo(file.targetFile)
}
