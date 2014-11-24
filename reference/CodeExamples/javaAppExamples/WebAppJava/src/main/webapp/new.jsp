<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<%@ taglib prefix="form" uri="http://www.springframework.org/tags/form" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF8">
<link rel="stylesheet" type="text/css" href="default.css"/>
<title>db4o Webapp Example</title>
</head>
<body>
<div id="page">
<p>
    	<h1>New Pilot</h1>
	<form:form method="post">

		Name:<br />
		<form:input path="name"/><br />
		Points<br />
		<form:input path="points"/><br />

		<input type="submit" class="button" value="Store"/><br/>
        <a href="list.html"/>Back</a>

	</form:form>

</p>
</div>
</body>
</html>