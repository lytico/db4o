package com.db4o.objectManager.v2.query;

import com.db4o.objectManager.v2.MainPanel;

import javax.swing.*;
import javax.swing.border.EmptyBorder;
import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;
import java.util.List;
import java.util.ArrayList;

/**
 * User: treeder
 * Date: Aug 21, 2006
 * Time: 7:17:06 PM
 */
public class QueryBarPanel extends JPanel {
    private JTextArea queryText;
    private JLabel queryStatus;
    private JComboBox queryList;
    static final String LAST_QUERY = "lastQuery";
    private static final String QUERY_HISTORY = "queryHistory";
    private MainPanel mainPanel;
    private List queryHistory;


    public QueryBarPanel(MainPanel mainPanel2) {
        super(new BorderLayout());
        this.mainPanel = mainPanel2;
        setBorder(new EmptyBorder(10, 10, 2, 10));
        add(new JLabel("Query:"), BorderLayout.WEST);

        // have drop down with last X queries
        queryHistory = (List) mainPanel.getPreferenceForDatabase(QUERY_HISTORY);
        ComboBoxModel queryListModel = new QueryHistoryComboBoxModel(queryHistory);
        queryList = new JComboBox(queryListModel);
		queryList.setRenderer(new QueryHistoryRenderer());
		queryList.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                JComboBox cb = (JComboBox) e.getSource();
                if (cb.getSelectedIndex() > 0) {
                    String queryString = (String) cb.getSelectedItem();
                    queryText.setText(queryString);
                }
            }
        });

        add(queryList, BorderLayout.NORTH);

        queryText = new JTextArea(5, 0);
        JScrollPane scrollPane = new JScrollPane(queryText);
        // preload with last query
        String lastQuery = (String) mainPanel.getPreferenceForDatabase(LAST_QUERY);
        if (lastQuery != null) {
            queryText.setText(lastQuery);
        }
        add(scrollPane, BorderLayout.CENTER);
        JButton submit = new JButton("  Submit  ");
        submit.addActionListener(new ActionListener() {
            public void actionPerformed(ActionEvent e) {
                // try to execute query
                String query = queryText.getText();
                if (query.length() > 0) {
                    addToQueryHistory(query);
                    //long startTime = System.currentTimeMillis();
                    //List<Result> results = Sql4o.execute(mainPanel.getObjectContainer(), query);
                    //long duration = System.currentTimeMillis() - startTime;
                    //setStatusMessage("Query duration: " + duration + "ms");
                    // instead of executing here like above, i'll pass the query along so the result table can do paging
                    mainPanel.displayResults(query);

                }
            }
        });
        add(new JPanel().add(submit), BorderLayout.EAST);
        queryStatus = new JLabel();
        add(queryStatus, BorderLayout.SOUTH);
    }


    private void addToQueryHistory(String query) {
        mainPanel.setPreferenceForDatabase(LAST_QUERY, query);
        List<String> x = queryHistory; // (List) mainPanel.getPreference(QUERY_HISTORY);
        if (x == null) x = new ArrayList();
        // remove any dupe query
        int exists = -1;
        for (int i = 0; i < x.size(); i++) {
            // not using contains, because will probably not be just a plain string in the future
            String s = x.get(i);
            if (s.equalsIgnoreCase(query)) {
                exists = i;
                break;
            }
        }
        if (exists == -1) x.add(0, query);
        else {
            // move to top
            String s = x.remove(exists);
            x.add(0, s);
        }
        mainPanel.setPreferenceForDatabase(QUERY_HISTORY, x);
    }

    private void setErrorMessage(String message) {
        queryStatus.setForeground(Color.RED);
        // todo: make bold - queryStatus.setFont(Font);
        queryStatus.setText(message);
    }

    private void setStatusMessage(String message) {
        queryStatus.setForeground(Color.BLACK);
        queryStatus.setText(message);
    }

    public JTextArea getQueryText() {
        return queryText;
    }

    public void showClassSummary(String className) {
        mainPanel.showClassSummary(className);
    }
}
