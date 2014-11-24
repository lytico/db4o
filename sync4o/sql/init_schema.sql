--
-- Initialization data for db4o Connector
-- $Id: init_schema.sql,v 1.2 2006/04/30 10:35:22 neil_macintosh Exp $
--

--
-- SyncSource Type registration
--
delete from fnbl_sync_source_type where id='sync4o';
insert into fnbl_sync_source_type(id, description, class, admin_class)
values('sync4o','Db4o SyncSource','com.db4o.sync4o.Db4oSyncSource', 'com.db4o.sync4o.ui.Db4oSyncSourceConfigPanel');

--
-- SyncConnector registration
--
delete from fnbl_connector where id='sync4o';
insert into fnbl_connector(id, name, description, admin_class)
values('sync4o','sync4o','sync4o','');

--
-- Connector the Connector registration to the SyncSource Type registration
--
delete from fnbl_connector_source_type where connector='sync4o';
insert into fnbl_connector_source_type(connector, sourcetype)
values('sync4o','sync4o');

--
-- SyncModule registration
--
delete from fnbl_module where id='sync4o';
insert into fnbl_module (id, name, description)
values('sync4o','sync4o','sync4o');

delete from fnbl_module_connector where module='sync4o' and connector='sync4o';
insert into fnbl_module_connector(module, connector)
values('sync4o','sync4o');
