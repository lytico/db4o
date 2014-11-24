package com.db4o.jiraui.soap;

import com.atlassian.jira.rpc.soap.client.JiraSoapService;
import com.atlassian.jira.rpc.soap.client.JiraSoapServiceService;
import com.atlassian.jira.rpc.soap.client.JiraSoapServiceServiceLocator;

import java.net.URL;
import java.rmi.RemoteException;
import javax.xml.rpc.ServiceException;

/**
 * This represents a SOAP session with JIRA including that state of being logged in or not
 */
public class SOAPSession
{
    private JiraSoapServiceService jiraSoapServiceLocator;
    private JiraSoapService jiraSoapService;
    private String token;

    public SOAPSession(URL webServicePort)
    {
        jiraSoapServiceLocator = new JiraSoapServiceServiceLocator();
        try
        {
            if (webServicePort == null)
            {
                jiraSoapService = jiraSoapServiceLocator.getJirasoapserviceV2();
            }
            else
            {
                jiraSoapService = jiraSoapServiceLocator.getJirasoapserviceV2(webServicePort);
            }
        }
        catch (ServiceException e)
        {
            throw new RuntimeException("ServiceException during SOAPClient contruction", e);
        }
    }

    public SOAPSession()
    {
        this(null);
    }

    public boolean connect(String userName, String password) throws RemoteException
    {
        token = getJiraSoapService().login(userName, password);
        return token != null;
    }

    public String getAuthenticationToken()
    {
        return token;
    }

    public JiraSoapService getJiraSoapService()
    {
        return jiraSoapService;
    }

    public JiraSoapServiceService getJiraSoapServiceLocator()
    {
        return jiraSoapServiceLocator;
    }
}
