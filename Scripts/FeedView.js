jQuery(function ($) {
    var localizedSettings = {};
    var service = {
        path: "CustomFeeds",
        framework: $.ServicesFramework(moduleId)
    }
    baseUrl = service.framework.getServiceRoot(service.path);
    // vraća krivi url pa se treba kemijat
    //getServiceRot = "gdi/Api/CustomFeeds"
    //ispravno = gdi/DesktopModules/CustomFeeds/API/Feed
alert(baseUrl);
    baseUrl = baseUrl.replace('API/CustomFeeds', 'DesktopModules/CustomFeeds/API/Feed');
alert(baseUrl);

    $("#customFeedsGrid").kendoGrid({
        dataSource: {
            type: "json",
            transport: {
                read: {
                    url: baseUrl + 'GetList',
                    dataType: "json",
                },
                update: {
                    url: baseUrl + 'Upsert',
                    dataType: "json",
                    type: "POST",
                    beforeSend: service.framework.setModuleHeaders,
                    complete: function (e) {
                        //TODO: handle errors
                    }
                },
                destroy: {
                    url: baseUrl + 'Delete',
                    dataType: "jsonp",
                    type: "POST",
                    beforeSend: service.framework.setModuleHeaders,
                    complete: function (e) {
                        //TODO: handle errors
                    }
                },
                create: {
                    url: baseUrl + 'Upsert',
                    dataType: "json",
                    type: "POST",
                    beforeSend: service.framework.setModuleHeaders,
                    complete: function (e) {
                        //TODO: handle errors
                        //if (e.responseJSON.status == 'error')     
                        //    alert(e.response);
                        //kendoGrid.dataSource.cancelChanges(); 
                        //else 

                        $("#customFeedsGrid").data("kendoGrid").dataSource.read();

                    }
                },
                parameterMap: function (parameters, operation) {
                    if (operation !== "read" && parameters.models) {
                        return { models: kendo.stringify(parameters.models) };
                    }
                    //else if (operation !== "read") {
                    //    return kendo.stringify(parameters);
                    //}
                    return parameters;
                }
            },
            pageSize: 20,
            schema: {
                data: function (response) {
                    return JSON.parse(response);
                },
                model: {
                    id: "FeedId",
                    fields: {
                        FeedId: { editable: false, nullable: true },
                        Title: { validation: { required: true } },
                        Address: { validation: { required: true } },
                        Description: { validation: { required: true } },
                        CreatedBy: { nullable: true },
                        CreatedDate: { nullable: true }
                    }
                }
            }
        },
        height: 550,
        groupable: false,
        sortable: true,
        editable: "popup",
        pageable: {
            refresh: true,
            pageSizes: true,
            buttonCount: 5
        },
        columns: [{ field: "Title", title: "Title" },
        {
            field: "Address",
            title: "Address",
            editor: function (container, options) {
                $('<input type="url" data-bind="value: ' + options.field + '"></input>').appendTo(container);
            }
        },
        {
            field: "Description", title: "Description",
            editor: function (container, options) {
                $('<textarea rows="3" data-bind="value: ' + options.field + '"></textarea>').appendTo(container);
            }
        },
        { field: "CreatedBy", title: "Created by", editor: noEditor },
        { field: "CreatedDate", title: "Date", template: '#= kendo.toString(kendo.parseDate(CreatedDate), "dd.MM.yyyy")#' },
        { command: ["edit", "destroy"], title: "&nbsp;", width: "250px" }],
        toolbar: ["create"],
        edit: function (e) {
            $(e.container).parent().css({
                width: '900px'
            });
            e.container.find("label[for=CreatedBy]").parent("div .k-edit-label").hide();
            e.container.find("label[for=CreatedBy]").parent().next("div .k-edit-field").hide();
            e.container.find("label[for=CreatedDate]").parent("div .k-edit-label").hide();
            e.container.find("label[for=CreatedDate]").parent().next("div .k-edit-field").hide();
        },
    });
    var noEditor = function (container, options) {
        debugger;
        container.prevObject.find("div[data-container-for='" + options.field + "']").hide();
        container.prevObject.find("label[for='" + options.field + "']").parent().hide();
    };
});