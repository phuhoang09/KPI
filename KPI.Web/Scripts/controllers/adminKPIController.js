﻿
$(document).ready(function () {
    adminKPIController.init();
    $("#addKPI .parent-search .search-box").hide();
});
var config = {
    pageSize: 3,
    pageIndex: 1
};
var adminKPIController = {
    init: function () {
        adminKPIController.loadData();
        adminKPIController.registerEvent();
        adminKPIController.loadDataCategory();
    },
    registerEvent: function () {
        $('#search').off('keypress').on('keypress', function (e) {
            if (e.which === 13) {
                adminKPIController.loadData(true);
            }
        });
        $('#table-group select').off('change').on('change', function (e) {
            adminKPIController.loadData(true);
        });
        //Save KPI
        $('#btnAdd').off('click').on('click', function () {
            adminKPIController.addData();

        });
        //Delete
        $('#btnDelete').off('click').on('click', function () {

        });
        //Update
        $('.btnUpdate').off('click').on('click', function () {
            //load detail
            adminKPIController.loadDetail(Number($(this).data('id')));
        });
        //save
        $('#btnSaveUpdateModal').off('click').on('click', function () {
            adminKPIController.updateData();
        });
        //delete
        $('.btnDelete').off('click').on('click', function () {
            adminKPIController.deleteData(Number($(this).data('id')));
        });
        //$('#addKPI #addKPI').typeahead({
        //    ajax: '/AdminKPI/Autocomplete'
        //});
        $('#addKPI .search').focusout(function () {
            $("#addKPI .parent-search .search-box").hide();
        });
        $("#addKPI .search").off('keyup').on('keyup', function (e) {
           
            $.ajax({
                type: "POST",
                url: "/AdminKPI/Autocomplete",
                data: 'name=' + $(this).val(),
                success: function (data) {
                    console.log(data);
                    if (data.length !== 0) {
                        $("#addKPI .parent-search .search-box").show();
                        $("#addKPI .parent-search .search-box ul").empty();
                        $.each(data, function (i, item) {
                            $("#addKPI .parent-search .search-box ul").append("<li>" + item + "</li>");

                        });
                    }
                }

            });
          
        });
    },
    log: function (message) {
        $("<div>").text(message).prependTo("#log");
        $("#log").scrollTop(0);
    },
    addData: function () {
        var res = adminKPIController.validate();
        if (res === false) {
            return false;
        }
        var mObj = {
            Code: $('#addKPI .Code').val(),
            Name: $('#addKPI .Name').val(),
            LevelID: $('#addKPI .LevelID').val(),
            CategoryID: $('#addKPI .Category').val()
        };
        $.ajax({
            url: "/AdminKPI/Add",
            data: JSON.stringify(mObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Add successfully!',
                        type: 'success',
                        confirmButtonText: 'OK'
                    });
                    $("#modal-group").modal('hide');
                    adminKPIController.loadData(true);
                    adminKPIController.resetForm();
                }
                else {
                    Swal.fire({
                        title: 'Error!',
                        text: 'This code has already existed!',
                        type: 'error'
                    });
                }

            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });
    },
    deleteData: function (id) {
        var ID = id;
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: "/AdminKPI/Delete/" + ID,
                    type: "POST",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {
                        Swal.fire({
                            title: 'Success!',
                            text: 'Delete successfully.',
                            type: 'success'
                        });
                        adminKPIController.loadData();
                        $("#modal-group").modal('hide');
                    },
                    error: function (errormessage) {
                        console.log(errormessage.responseText);
                    }
                });
                Swal.fire(
                    'Deleted!',
                    'Reacord has been deleted.',
                    'success'
                );
            }
        });

    },
    resetForm: function () {
        $('.ID').val("");
        $('.Name').val("");
        $('.LevelID').val("");
        $('.Category').val("");
        $('.Name').css('border-color', 'lightgrey');
        $('.LevelID').css('border-color', 'lightgrey');
        $('.Category').css('border-color', 'lightgrey');
    },
    updateData: function () {
        console.log(1);
        var mObj = {
            ID: $('#modal-group2 .ID').val(),
            Name: $('#modal-group2 .Name').val(),
            Code: $('#modal-group2 .Code').val(),
            LevelID: $('#modal-group2 .LevelID').val(),
            CategoryID: $('#modal-group2 .Category').val()
        };

        $.ajax({
            url: "/AdminKPI/Update",
            data: JSON.stringify(mObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Update successfully!',
                        type: 'success'
                    });
                    adminKPIController.resetForm();
                    $("#modal-group2").modal('hide');
                    adminKPIController.loadData();

                }

            },
            error: function (error) {
                console.log(error);
            }
        });
    },
    loadDetail: function (id) {
        var value = id;
        $.ajax({
            url: "/AdminKPI/GetbyID/",
            data: { ID: JSON.stringify(value) },
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                $('#modal-group2 .ID').val(result.ID);
                $('#modal-group2 .Name').val(result.Name);
                $('#modal-group2 .Code').val(result.Code);
                $('#modal-group2 .LevelID').val(result.LevelID);
                $('#modal-group2 .Category').val(result.CategoryID);
                $('#modal-group2').modal('show');

            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    loadDataCategory: function () {
        $.ajax({
            url: '/AdminKPI/ListCategory',
            type: "GET",
            dataType: "json",
            success: function (data) {
                $("#table-group select").empty();
                $("#table-group select").append('<option value="">.: Choose Category :.</option>');
                $(".Category").empty();
                $(".Category").append('<option value="">.: Choose Category :.</option>');
                //console.log(data);
                $.each(data, function (key, item) {
                    $("#table-group select").append("<option value=\"" + item.ID + "\">" + item.Name + "</option>");
                    $(".Category").append("<option value=\"" + item.ID + "\">" + item.Name + "</option>");
                });
            }
        });
    },
    validate: function () {
        var isValid = true;
        if ($('.Name').val().trim() === "") {
            $('.Name').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.Name').css('border-color', 'lightgrey');
        }
        if ($('.LevelID').val().trim() === "") {
            $('.LevelID').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.LevelID').css('border-color', 'lightgrey');
        }
        if ($('#Category').val().trim() === "") {
            $('#Category').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#Category').css('border-color', 'lightgrey');
        }
        return isValid;
    },
    loadData: function (changePageSize) {
        var name = $('#search').val();
        var categoryId = $('#table-group select').find(':selected').val();
        $.ajax({
            url: '/AdminKPI/LoadData',
            type: 'GET',
            data: {
                catID: categoryId,
                name: name,
                page: config.pageIndex,
                pageSize: config.pageSize
            },
            dataType: 'json',
            beforeSend: function () {
                $("#main-loading-delay").show();
            },
            success: function (response) {
                if (response.status) {
                    $("#main-loading-delay").hide();
                    var count = 1;
                    //console.log(response.data);
                    var data = response.data;
                    var html = '';
                    var template = $('#tblkpi-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            No: count,
                            Name: item.Name,
                            Code: item.Code,
                            Level: item.LevelID,
                            CategoryName: item.CategoryName,
                            ID: item.ID,
                            IDDelete: item.ID
                        });
                        count++;
                    });
                    $('#tblkpi').html(html);
                    adminKPIController.paging(response.total, function () {
                        adminKPIController.loadData();
                    }, changePageSize);
                    adminKPIController.registerEvent();
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / config.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage === 0 ? 1 : totalPage,
            first: "First",
            next: "Next",
            last: "Last",
            prev: "Previous",
            visiblePages: 10,
            onPageClick: function (event, page) {
                config.pageIndex = page;
                setTimeout(callback, 500);
            }
        });
    }
};
