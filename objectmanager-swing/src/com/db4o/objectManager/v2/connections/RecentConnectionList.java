package com.db4o.objectManager.v2.connections;

import com.db4o.objectmanager.api.prefs.Preferences;
import com.db4o.objectmanager.model.Db4oConnectionSpec;
import com.db4o.objectmanager.model.Db4oFileConnectionSpec;
import com.db4o.objectManager.v2.connections.ConnectionListCellRenderer;

import javax.swing.*;
import java.awt.Component;
import java.awt.BorderLayout;
import java.util.List;
import java.util.ArrayList;
import java.io.File;

/**
 * User: treeder
 * Date: Aug 8, 2006
 * Time: 11:50:33 PM
 */
public class RecentConnectionList extends JPanel {
    private JList list;
    private DefaultListModel listModel;
    private static final String RECENT_CONNECTIONS = "recentConnections";


    public RecentConnectionList() {
        super(new BorderLayout());
        List<Db4oConnectionSpec> connections = getRecentConnectionSpecsFromDb();
        listModel = new DefaultListModel();
        for (int i = 0; i < connections.size(); i++) {
            Db4oConnectionSpec db4oConnectionSpec = connections.get(i);
            listModel.addElement(db4oConnectionSpec);
        }
        list = new JList(listModel);
        list.setCellRenderer(new ConnectionListCellRenderer());
        list.setVisibleRowCount(5);
        JScrollPane listScroller = new JScrollPane(list);
        this.add(listScroller);
    }

    private List<Db4oConnectionSpec> getRecentConnectionSpecsFromDb() {
        List<Db4oConnectionSpec> connections = (List<Db4oConnectionSpec>) Preferences.getDefault().getPreference(RECENT_CONNECTIONS);
        if (connections == null) connections = new ArrayList<Db4oConnectionSpec>();
		// now clean out connections where the file does not exist
		int removed = 0;
		for (int i = 0; i < connections.size(); i++) {
			Db4oConnectionSpec connectionSpec = connections.get(i);
			if(connectionSpec instanceof Db4oFileConnectionSpec){
				File f = new File(connectionSpec.getFullPath());
				if(!f.exists()){
					connections.remove(connectionSpec);
					i--;
					removed++;
				}
			}
		}
		if(removed > 0){
			saveConnections(connections);
		}
		return connections;
    }

    public void addNewConnectionSpec(Db4oConnectionSpec connectionSpec) {
        // make sure it's not already here
		List<Db4oConnectionSpec> connections = getRecentConnectionSpecsFromDb();
		for (int i = 0; i < listModel.getSize(); i++) {
            Db4oConnectionSpec spec = (Db4oConnectionSpec) listModel.get(i);
            if (spec.getPath().equals(connectionSpec.getPath())) {
				// move to top of list
				listModel.remove(i);
				listModel.add(0, spec);
				list.setSelectedIndex(0);
				connections.remove(spec);
				connections.add(0, spec);
				saveConnections(connections);
				return;
            }
        }
        connections.add(0, connectionSpec);
		saveConnections(connections);
		listModel.addElement(connectionSpec);
    }

	private void saveConnections(List<Db4oConnectionSpec> connections) {
		Preferences.getDefault().setPreference(RECENT_CONNECTIONS, connections);
	}

	public Db4oConnectionSpec getSelected() {
        if (list.getSelectedIndex() != -1) {
            return (Db4oConnectionSpec) listModel.get(list.getSelectedIndex());
        }
        return null;
    }

    public Component getList() {
        return list;
    }
}
