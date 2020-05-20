
// Extend the Polygon class with our own version of the missing function, taken from:
// https://stackoverflow.com/questions/3081021/how-to-get-the-center-of-a-polygon-in-google-maps-v3
google.maps.Polygon.prototype.customGetBounds = function () {
    var bounds = new google.maps.LatLngBounds()
    this.getPath().forEach(function (element, index) { bounds.extend(element) })
    return bounds;
}

var RbatApp = RbatApp || {};

RbatApp.Map = RbatApp.Map || (function () {

    var map;
    var overlay; //see http://stackoverflow.com/questions/5510972/google-maps-drag-and-drop-objects-into-google-maps-from-outside-the-map
    var drawingManager;
    var allShapes = [];
    var DEFAULTMAPCENTER = new google.maps.LatLng(51.8, -113.5); //hard coded Alberta location by default
    var DEFAULTMAPZOOM = 7;
    var DEFAULTCIRCLERADIUS = 10000; //meters
    var JUNCTIONNODETYPEID = 3;
    var editableFlag = false;

    // when an object is dragged into the map.
    function dragInside(e, item) {
        var x = e.pageX - $('#map').offset().left;
        var y = e.pageY - $('#map').offset().top;
        if (x > 0) {
            var point = new google.maps.Point(x, y);
            var position = overlay.getProjection().fromContainerPixelToLatLng(point);
            // add position to the object
            item.mapData = {};
            item.mapData.mapPosition = [position.lat(), position.lng()];
            item.mapData.radius = Number($("#circleRadius").val());
        }
    }

    function getNodesElements() {
        return $('#nodes .list-group-item');
    }

    function getChannelsElements() {
        return $('#channels .list-group-item');
    }

    function disableNode(id) {
        $("#nodes #" + id).addClass("disabled").css("background-color", "#E6E6E6");
    }

    function enableNode(id) {
        $("#nodes #" + id).removeClass("disabled").css("background-color", "#fff");
    }

    function disableChannel(id) {
        $("#channels #" + id).addClass("disabled").css("background-color", "#E6E6E6");
    }

    function enableChannel(id) {
        $("#channels #" + id).removeClass("disabled").css("background-color", "#fff");
    }

    function getElementInfo(element) {
        var id = element.attr('id');
        var name = element.text();
        var typeId = element.data('type');
        return { id, name, typeId };
    }

    function hookupNodesDragEvents() {
        getNodesElements().draggable({
            cancel: ".disabled",
            revert: true,
            revertDuration: 0,
            cursor: "pointer",
            stop: function (e, ui) {
                e.preventDefault();
                var nodeElement = $(this);
                var node = getElementInfo(nodeElement);
                if (node.typeId === JUNCTIONNODETYPEID) {
                    dragInside(e, node);
                    generateCircle(node);
                } else {
                    initiatePolygon(node);
                }
            }
        });
    }

    function hookupChannelsDragEvents() {
        getChannelsElements().draggable({
            cancel: ".disabled",
            revert: true,
            revertDuration: 0,
            stop: function (e, ui) {
                e.preventDefault();
                var channelElement = $(this);
                var channel = getElementInfo(channelElement);
                initiateLine(channel);
            }
        });
    }

    function resetNodesDraggable() {
        getNodesElements().draggable("destroy");
        hookupNodesDragEvents();
    }

    function resetChannelsDraggable() {
        getChannelsElements().draggable("destroy");
        hookupChannelsDragEvents();
    }

    function deleteShape(shape) {
        shape.setMap(null);
        shape = null;
    }

    function generateContextMenuEvent(shape, callback) {
        google.maps.event.addListener(shape, 'rightclick', function (event) {
            var contextMenuId = shape.item + shape.itemId;
            if ($("#map #" + contextMenuId).length === 0) {
                var contextMenu = $(document.createElement('nav'));
                contextMenu.attr("id", contextMenuId);
                contextMenu.addClass('contextmenu nav flex-column nav-pills');
                var deleteButton = $("<a class='nav-item nav-link' href='#'>Delete</a>");
                contextMenu.append(deleteButton);
                if (shape.item === "node") {
                    var editButton = $("<a class='nav-item nav-link' href='Node?nodeId=" + shape.itemId + "' target='_blank'>Edit</a>");
                    editButton.click(function () {
                        contextMenu.remove();
                    });
                    contextMenu.append(editButton);
                } else {
                    var arrowButton;
                    if (shape.icons.length === 0) {
                        arrowButton = $("<a class='nav-item nav-link' href='#'>Add Arrow</a>");
                        arrowButton.click(function () {``
                            contextMenu.remove();
                            shape.set('icons', [{
                                icon: {
                                    path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW
                                }
                            }]);
                        });
                    } else {
                        arrowButton = $("<a class='nav-item nav-link' href='#'>Remove Arrow</a>");
                        arrowButton.click(function () {
                            contextMenu.remove();
                            shape.set('icons', []);
                        });
                    }
                    contextMenu.append(arrowButton);
                }
                
                var chartButton = $("<a class='nav-item nav-link' href='ReportingTools/Chart?projectId=" + $('#projectList :selected').val() + "&" + shape.item + "Id=" + shape.itemId + "' target='_blank'>Chart</a>");
                contextMenu.append(chartButton);
                var pos = overlay.getProjection().fromLatLngToContainerPixel(event.latLng);
                contextMenu.css("left", pos.x);
                contextMenu.css("top", pos.y);
                contextMenu.css("position", "absolute");
                deleteButton.click(function () {
                    contextMenu.remove();
                    allShapes.splice(allShapes.indexOf(shape), 1);
                    deleteShape(shape);
                    callback();
                });
                $("#map").append(contextMenu);
            }
        });
    }

    function getInfoWindowPosition(shape) {
        if (shape.itemType === JUNCTIONNODETYPEID && shape.item === "node") { // circle
            return shape.getCenter();
        } else if (shape.item === "node") { //polygon
            return shape.customGetBounds().getCenter();
        } else if (shape.item === "channel") { // polyline
            var coords = shape.getPath();
            var coordsLendth = coords.length;
            var middle = Math.floor(coordsLendth / 2);
            return coords.getAt(middle);
        }
    }

    function addTitleOnMouseOver(shape) {
        google.maps.event.addListener(shape, 'mouseover', function () {
            map.getDiv().setAttribute('title', shape.itemName);
        });

        google.maps.event.addListener(shape, 'mouseout', function () {
            map.getDiv().removeAttribute('title');
        });

        var position = getInfoWindowPosition(shape);
        var infoWindow = new google.maps.InfoWindow({
            content: shape.itemName,
            position: position
        });

        shape.infoWindow = infoWindow;
    }

    function addCommonNodeShapeEvents(shape) {
        addTitleOnMouseOver(shape);

        generateContextMenuEvent(shape, function () {
            enableNode(shape.itemId);
            resetNodesDraggable();
        });

        if (allShapes.indexOf(shape) < 0) {
            allShapes.push(shape);
        }

        disableNode(shape.itemId);
    }

    function resolveNodeColor(nodeTypeId) {
        if (nodeTypeId === 1) {
            return "#0000FF"; //Reservoir = blue
        }
        if (nodeTypeId === 2) {
            return "#008000"; //Consumptive Usel = green
        }
        if (nodeTypeId === 3) {
            return "#838383"; //Junction = dark grey
        }
        return "";
    }

    function resolveChannelColor(channelTypeId) {
        if (channelTypeId === 1) {
            return "#0000FF"; //River reach = blue
        }
        if (channelTypeId === 2) {
            return "#A52A2A"; //Diversion channel = brown
        }
        if (channelTypeId === 3) {
            return "#838383"; //Return flow = dark grey
        }
        return "";
    }

    function getCommonShapeOptions(data, dataType) {
        return {
            item: dataType,
            itemId: data.id,
            itemName: data.name,
            itemType: data.typeId,
            strokeColor: dataType === "node" ? resolveNodeColor(data.typeId) : resolveChannelColor(data.typeId),
            strokeOpacity: 0.8,
            strokeWeight: 3,
            fillColor: resolveNodeColor(data.typeId),
            fillOpacity: 0.35,
            editable: editableFlag
        };
    }

    function generateCircle(node) {
        var opts = getCommonShapeOptions(node, "node");
        opts.center = new google.maps.LatLng(node.mapData.mapPosition[0], node.mapData.mapPosition[1]);
        opts.radius = node.mapData.radius || DEFAULTCIRCLERADIUS;
        opts.map = map;
        // Add a circle for this node to the map.
        var nodeCircle = new google.maps.Circle(opts);

        addCommonNodeShapeEvents(nodeCircle);
    }
    
    function getPolyLineOptions(channel) {
        var opts = getCommonShapeOptions(channel, "channel");
        opts.map = map;
        opts.geodesic = true;
        opts.strokeOpacity = 1.0;
        opts.icons = channel.mapData && channel.mapData.icons ? channel.mapData.icons : [];
        return opts;
    }

    function setupLine(shape) {
        addTitleOnMouseOver(shape);

        generateContextMenuEvent(shape, function () {
            enableChannel(shape.itemId);
            resetChannelsDraggable();
        });
        if (allShapes.indexOf(shape) < 0) {
            allShapes.push(shape);
        }

        disableChannel(shape.itemId);
    }

    function initiateLine(channel) {
        // Set the drawing mode.
        drawingManager.setOptions({
            polylineOptions: getPolyLineOptions(channel)
        });
        drawingManager.setDrawingMode(google.maps.drawing.OverlayType.POLYLINE);
    }

    function drawLine(channel) {
        var polyline = new google.maps.Polyline(getPolyLineOptions(channel));
        polyline.setPath(channel.mapData.mapPosition);
        setupLine(polyline);
    }

    function getPolygonOptions(node) {
        var opts = getCommonShapeOptions(node, "node");
        opts.map = map;
        return opts;
    }

    function initiatePolygon(node) {
        // Set the drawing mode.
        drawingManager.setOptions({
            polygonOptions: getPolygonOptions(node)
        });
        drawingManager.setDrawingMode(google.maps.drawing.OverlayType.POLYGON);
    }

    function drawPolygon(node) {
        var polygon = new google.maps.Polygon(getPolygonOptions(node));
        polygon.setPath(node.mapData.mapPosition);
        addCommonNodeShapeEvents(polygon);
    }

    function initialize() {
        map = new google.maps.Map(document.getElementById("map"), {
            center: DEFAULTMAPCENTER,
            zoom: DEFAULTMAPZOOM//,
            // mapTypeId: 'roadmap'
        });
        overlay = new google.maps.OverlayView();
        overlay.draw = function () { };
        overlay.setMap(map);

        drawingManager = new google.maps.drawing.DrawingManager({
            drawingMode: null,
            drawingControl: false
        });
        drawingManager.setMap(map);

        hookupNodesDragEvents();
        hookupChannelsDragEvents();

        google.maps.event.addListener(map, 'click', function () {
            $(".contextmenu").remove();
        });

        google.maps.event.addListener(drawingManager, 'polygoncomplete', function (polygon) {
            drawingManager.setDrawingMode(null); // Return to 'hand' mode
            addCommonNodeShapeEvents(polygon);
        });

        google.maps.event.addListener(drawingManager, 'polylinecomplete', function (polyline) {
            drawingManager.setDrawingMode(null); // Return to 'hand' mode
            setupLine(polyline);
        });

        $("#projectList").change();
    }

    function getMapPosition(shape) {
        var mapPosition = [];

        var path = shape.getPath();
        var i;

        for (i = 0; i < path.getLength(); i++) {
            mapPosition.push({
                lat: path.getAt(i).lat(),
                lng: path.getAt(i).lng()
            });
        }
        return mapPosition;
    }

    function getShape(elementId, elementType) {
        var i;
        var shape;
        for (i = 0; i < allShapes.length; i++) {
            shape = allShapes[i];
            if (shape.item === elementType && shape.itemId.toString() === elementId) {
                if (shape.item === "node" && shape.itemType === JUNCTIONNODETYPEID) { // Junction == circle
                    return JSON.stringify({
                        radius: shape.getRadius(),
                        mapPosition: [shape.getCenter().lat(), shape.getCenter().lng()]
                    });
                } else if (shape.item === "channel") {
                    return JSON.stringify({
                        icons: shape.icons,
                        mapPosition: getMapPosition(shape)
                    });
                } else {
                    return JSON.stringify({
                        mapPosition: getMapPosition(shape)
                    });
                }
            }
        }
        return null;
    }

    function getMapData() {
        var center = map.getCenter();
        var nodes = [];
        var channels = [];
        var i;
        var element;
        var nodeShape;
        var projectMapData = {
            center: [center.lat(), center.lng()],
            zoom: map.getZoom(),
            junctionRadius: $("#circleRadius").val()
        };

        for (i = 0; i < getNodesElements().length; i++) {
            element = getElementInfo($(getNodesElements()[i]));
            nodeShape = getShape(element.id, "node");
            nodes.push({
                id: element.id,
                name: element.name,
                typeId: element.typeId,
                mapData: nodeShape
            });
        }

        for (i = 0; i < getChannelsElements().length; i++) {
            element = getElementInfo($(getChannelsElements()[i]));
            nodeShape = getShape(element.id, "channel");
            channels.push({
                id: element.id,
                name: element.name,
                typeId: element.typeId,
                mapData: nodeShape
            });
        }

        return {
            projectMapData: JSON.stringify(projectMapData),
            projectNodes: nodes,
            projectChannels: channels
        };
    }

    function getElementIcon(selector, listItem) {
        if (selector === "#nodes") {
            if (listItem.typeId === 3) {
                return "<i class='far fa-fw fa-circle' style='color: " + resolveNodeColor(listItem.typeId) + "'></i>";
            } else {
                return "<i class='fas fa-fw fa-draw-polygon' style='color: " + resolveNodeColor(listItem.typeId) + "'></i>";
            }
        } else {
            return "<i class='fas fa-fw fa-long-arrow-alt-up' style='color: " + resolveChannelColor(listItem.typeId) + "'></i>";
        }
    }

    function resetList(selector, newList) {
        var i;
        var liElements = [];
        var listItem;
        var liElement;
        for (i = 0; i < newList.length; i++) {
            listItem = newList[i];
            liElement = "<li id='" + listItem.id + "' class='list-group-item' data-type='" + listItem.typeId + "'>" + getElementIcon(selector, listItem) + "&nbsp; " + listItem.name + "</li>";
            liElements.push(liElement);
        }
        $(selector).empty().append(liElements);
    }

    function loadMapData(projectData) {
        var i;
        var node;
        var channel;
        var item;

        resetList("#nodes", projectData.projectNodes);
        resetList("#channels", projectData.projectChannels);

        for (i = 0; i < allShapes.length; i++) {
            deleteShape(allShapes[i]);
        }

        hookupNodesDragEvents();

        hookupChannelsDragEvents();

        var projectMapDataJson;
        if (projectData.projectMapData) {
            projectMapDataJson = JSON.parse(projectData.projectMapData);
        }
        if (projectMapDataJson && projectMapDataJson.center) {
            map.setCenter(new google.maps.LatLng(projectMapDataJson.center[0], projectMapDataJson.center[1]));
        } else {
            map.setCenter(DEFAULTMAPCENTER);
        }
        if (projectMapDataJson && projectMapDataJson.zoom) {
            map.setZoom(projectMapDataJson.zoom);
        } else {
            map.setZoom(DEFAULTMAPZOOM);
        }

        $("#circleRadius").val(projectMapDataJson && projectMapDataJson.junctionRadius ? projectMapDataJson.junctionRadius : DEFAULTCIRCLERADIUS);

        allShapes = [];

        for (i = 0; i < projectData.projectNodes.length; i++) {
            item = projectData.projectNodes[i];
            if (item.mapData && JSON.parse(item.mapData)) {
                item.mapData = JSON.parse(item.mapData);
                if (item.typeId === JUNCTIONNODETYPEID) { // Junction ==> circle
                    generateCircle(item);
                } else {
                    drawPolygon(item); // Reservoir or Consumptive Use ==> polygon
                }
            }
        }

        for (i = 0; i < projectData.projectChannels.length; i++) {
            item = projectData.projectChannels[i];
            if (item.mapData && JSON.parse(item.mapData)) {
                item.mapData = JSON.parse(item.mapData);
                drawLine(item);
            }
        }
    }

    function makeMapDataEditable(editable) {
        editableFlag = editable;
        var i;
        for (i = 0; i < allShapes.length; i++) {
            allShapes[i].setEditable(editable);
        }
    }

    function showAllShapeNames(show) {
        var i;
        if (show) {
            for (i = 0; i < allShapes.length; i++) {
                allShapes[i].infoWindow.open(map);
            }
        } else {
            for (i = 0; i < allShapes.length; i++) {
                allShapes[i].infoWindow.close();
            }
        }
    }

    google.maps.event.addDomListener(window, 'load', initialize);

    document.addEventListener("contextmenu", function (evt) {
        evt.preventDefault();
    });

    return {
        getMapData,
        loadMapData,
        makeMapDataEditable,
        showAllShapeNames
    };
}());

(function () {
    $("#projectList").change(function () {
        var projectId = $('#projectList :selected').val();
        if (projectId) {
            $("#radiusSection").show();
            $("#actionMenu").show();
        } else {
            $("#radiusSection").hide();
            $("#actionMenu").hide();
        }
        $.ajax({
            url: "Map/GetProjectData",
            data: {
                projectId
            }
        }).done(function (projectData) {
            RbatApp.Map.loadMapData(projectData);
        });
    });

    $("#showAllNames").change(function (event) {
        var checked = event.target.checked;
        RbatApp.Map.showAllShapeNames(checked);
    });

    function showAlert(alertID, text) {
        $(alertID).html(text);
        $(alertID).removeClass("in").show();
        $(alertID).delay(200).addClass("in").fadeOut(5000);
        $(alertID).show();
    }

    function showSuccessfullySaved(text) {
        showAlert("#savedAlert", text);
    }

    function showNotSaved(text) {
        showAlert("#badInputAlert", text);
    }

    $("#saveButton").click(function () {
        RbatApp.Map.makeMapDataEditable(false);
        $("#editButton").text("Edit Mode");
        var projectId = $('#projectList :selected').val();
        if (projectId) {
            var mapData = RbatApp.Map.getMapData();
            mapData.projectId = projectId;
            $.ajax({
                url: "Map/SaveProjectData",
                data: mapData,
                method: 'POST'
            }).done(function (result) {
                if (result.type === "Success") {
                    showSuccessfullySaved("Changes saved");
                } else {
                    showNotSaved("Error has occurred. " + result.message ? result.message : "");
                }
            });
        }
    });

    $("#editButton").click(function () {
        var projectId = $('#projectList :selected').val();
        if (projectId) {
            var editMode = $("#editButton").text() === "Read Mode";
            $("#editButton").text(editMode ? "Edit Mode" : "Read Mode");
            RbatApp.Map.makeMapDataEditable(!editMode);
        }
    });
}());
