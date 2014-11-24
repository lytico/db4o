<%@ taglib uri="http://java.sun.com/jsp/jstl/core" prefix="c" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF8">
    <link rel="stylesheet" type="text/css" href="default.css"/>
    <title>db4o Webapp Example</title>

</head>
<body>
<div id="page">
    <h1>db4o Webapp Example</h1>

    <p>

    <h2>Pilot List</h2>

    <div id="content">
        <table class="list">
            <tr>
                <th></th>
                <th>
                    Name
                </th>
                <th>
                    Points
                </th>
            </tr>            
            <!-- #example: In the view use the ids to identify the objects#-->
            <c:forEach items="${pilots}" var="pilot">
                <tr>
                    <td>
                        <a href="edit${pilot.id}.html"/>Edit</a>
                        <a href="delete${pilot.id}.html"/>Delete</a>
                    </td>
                    <td>
                            ${pilot.name}
                    </td>
                    <td>
                            ${pilot.points}
                    </td>
                </tr>
            </c:forEach>
            <!-- #end example -->
        </table>
    </div>

    <a href="new.html">Create Pilot</a>

</div>
</body>
</html>