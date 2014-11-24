package com.db4o.objectManager.v2.query;

import com.db4o.objectManager.v2.results.CollectionValue;

import javax.swing.*;
import java.awt.Point;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;

/**
 * User: treeder
 * Date: Sep 7, 2006
 * Time: 10:42:29 PM
 */
public class QueryResultsTableSelectionListener extends MouseAdapter {
    JTable table;
    private QueryResultsPanel queryResultsPanel;

    // It is necessary to keep the table since it is not possible
    // to determine the table from the event's source
    public QueryResultsTableSelectionListener(JTable table, QueryResultsPanel queryResultsPanel) {
        this.table = table;
        this.queryResultsPanel = queryResultsPanel;
    }

    public void mousePressed(MouseEvent e) {
        JTable table = (JTable) e.getSource();
        Point p = e.getPoint();
        int row = table.rowAtPoint(p);
        int col = table.columnAtPoint(p);
        Object value =  table.getValueAt(row, col);
        QueryResultsTableModel model = (QueryResultsTableModel) table.getModel();
        if(col == QueryResultsTableModel.COL_TREE){
            queryResultsPanel.showObjectTree(model.getRowObject(row));
        }
        if(e.getClickCount() == 2 && value instanceof CollectionValue){
            // show collection on double click
            queryResultsPanel.showObjectTree(model.getRowObject(row));
        }
    }


}
