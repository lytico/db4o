package com.db4o.omplus.datalayer;
import java.io.IOException;
import java.net.URL;

import org.eclipse.core.runtime.FileLocator;
import org.eclipse.core.runtime.Platform;
import org.eclipse.jface.resource.ImageDescriptor;
import org.eclipse.swt.graphics.Image;
import org.osgi.framework.Bundle;


public class ImageUtility
{
 /* private static ImageRegistry image_registry;

  public static URL newURL(String url_name)
  {
    try
    {
      return new URL(url_name);
    }
    catch (MalformedURLException e)
    {
      throw new RuntimeException("Malformed URL " + url_name, e);
    }
  }

  public static ImageRegistry getImageRegistry()
  {
    if (image_registry == null)
    {
      image_registry = new ImageRegistry();    	
      //image_registry.put("folder",ImageDescriptor.createFromURL(newURL("file:icons/folder.gif")));
      //image_registry.put("file",ImageDescriptor.createFromURL(newURL("file:icons/file.gif")));
      //image_registry.put("file",ImageDescriptor.createFromFile(Utility.class, "hehe/close.gif"));
      //ImageDescriptor imageDesc = ImageDescriptor.createFromURL(newURL("file:/icons/pp.gif"));
      //ImageDescriptor imageDesc = ImageDescriptor.createFromFile(Utility.class, "hehe/close.gif");
      //image_registry.put("file",(ImageDescriptor)null );
      
      //TODO: need to ideally take relative paths here.
      image_registry.put(OMPlusConstants.COLUMN_VISIBLE_ICON,ImageDescriptor.createFromFile(ImageUtility.class, "srcIcons/column_visible.jpg"));
      image_registry.put(OMPlusConstants.PRIMITIVE_ICON,ImageDescriptor.createFromFile(ImageUtility.class, "srcIcons/primitive.jpg"));
      image_registry.put(OMPlusConstants.COLLECTION_ICON,ImageDescriptor.createFromFile(ImageUtility.class, "srcIcons/collection.jpg"));
      image_registry.put(OMPlusConstants.COMPLEX_ICON,ImageDescriptor.createFromFile(ImageUtility.class, "srcIcons/complex.jpg"));
    }
    return image_registry;
    
    
  }*/
	private static Bundle bundle = Platform.getBundle(OMPlusConstants.PLUGIN_ID);
	private static String mainIconPath = "icons/";
	
	private static final String COLUMN_VISIBLE_ICON_NAME = "column_visible.jpg";
	private static final String PRIMITIVE_ICON_NAME = "primitive.gif";
	private static final String COLLECTION_ICON_NAME = "collection.gif";
	private static final String COMPLEX_ICON_NAME = "complex.gif";
	
	private static final String FLAT_VIEW_ICON_NAME = "flat_view.gif";
	private static final String HIERARCHICAL_VIEW_ICON_NAME = "hierarchical_view.gif";
	private static final String DB4O_LOGO_IMAGE_NAME = "db4o1.bmp";
	private static final String CLOSE_GROUP_ICON_NAME = "close_group.gif";
	private static final String CLEAR_FILTER_ICON_NAME = "clear_filter.gif";
	private static final String BROWSE_ICON_NAME = "Browse.png";
	
	private static final String SEARCH_NEXT_ICON_NAME = "forward.gif";
	private static final String SEARCH_PREV_ICON_NAME = "back.gif";
	private static final String DB4O_WIND_ICON_NAME = "db4oicon.bmp";
	
	//TODO: every function here dows a lot of common task. Move to a single function
	public static Image getImage(int imageId)
	{
		switch(imageId)
		{
			case OMPlusConstants.COLUMN_VISIBLE_ICON:
				return getNImage(COLUMN_VISIBLE_ICON_NAME);
			case OMPlusConstants.PRIMITIVE_ICON:
				return getNImage(PRIMITIVE_ICON_NAME);
			case OMPlusConstants.COLLECTION_ICON:
				return getNImage(COLLECTION_ICON_NAME);
			case OMPlusConstants.COMPLEX_ICON:
				return getNImage(COMPLEX_ICON_NAME);
			case OMPlusConstants.FLAT_VIEW_ICON:
				return getNImage(FLAT_VIEW_ICON_NAME);
			case OMPlusConstants.HIERARCHICAL_VIEW_ICON:
				return getNImage(HIERARCHICAL_VIEW_ICON_NAME);
			case OMPlusConstants.DB4O_LOGO_IMAGE:
				return getNImage(DB4O_LOGO_IMAGE_NAME);
			case OMPlusConstants.CLOSE_GROUP_ICON:
				return getNImage(CLOSE_GROUP_ICON_NAME);
			case OMPlusConstants.BROWSE_ICON:
				return getNImage(BROWSE_ICON_NAME);
			case OMPlusConstants.CLEAR_FILTER_ICON:
				return getNImage(CLEAR_FILTER_ICON_NAME);
			case OMPlusConstants.SEARCH_NEXT_ICON:
				return getNImage(SEARCH_NEXT_ICON_NAME);	
			case OMPlusConstants.SEARCH_PREV_ICON:
				return getNImage(SEARCH_PREV_ICON_NAME);
			case OMPlusConstants.DB4O_WIND_ICON:
				return getNImage(DB4O_WIND_ICON_NAME);
			default: 
				return null;				
		}
	}
	
	private static Image getNImage(String imageName) {
		String path = mainIconPath + imageName;
		URL entry = bundle.getEntry(path);
		URL url = null;
		try 
		{
			url = FileLocator.resolve(entry);
		}
		catch (IOException e) 
		{
			e.printStackTrace();
		}
		ImageDescriptor imagedesc = ImageDescriptor.createFromURL(url);
		if(imagedesc != null)
			return imagedesc.createImage();
		else
			return null;
	}

	public static Image  getCollectionIcon()
	{
		String path = mainIconPath + COLLECTION_ICON_NAME;
		URL entry = bundle.getEntry(path);
		URL url = null;
		try 
		{
			url = FileLocator.resolve(entry);
		}
		catch (IOException e) 
		{
			e.printStackTrace();
		}
		ImageDescriptor imagedesc = ImageDescriptor.createFromURL(url);
		if(imagedesc != null)
			return imagedesc.createImage();
		else
			return null;
	}
	
}
