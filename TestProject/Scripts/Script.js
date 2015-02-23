var app = angular.module('main', ['ngRoute', 'daterangepicker', 'ngTable', 'validation', 'validation.rule', 'ui.bootstrap']).
controller('DemoCtrl', function ($scope, $http, $filter, $location, ngTableParams) {

    $scope.roles = {};

    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 10,          // count per page
        filter: {
            FilterDateOfBirth : { startDate: null, endDate: null },
            FilterAddedDate: { startDate: null, endDate: null },
            FilterActivatedDate: { startDate: null, endDate: null },
            FilterLastVisitDate: { startDate: null, endDate: null }
},
        sorting: {
            ID: 'asc'     // initial sorting
        }
    }, {

        getData: function ($defer, params) {
            for (var i in params.sorting()) {
                $scope.SortColumn = i;
                $scope.SortOrder = params.sorting()[i];
            }
            $scope.myDateRange;
            var p = {
                PageNumber: $scope.tableParams.page(),
                PageSize: $scope.tableParams.count(),
                SortColumn: $scope.SortColumn,
                SortOrder: $scope.SortOrder,
                FilterName: params.filter()['Name'],
                FilterID: params.filter()['ID'],
                FilterEmail: params.filter()['Email'],
                FilterLastName: params.filter()['LastName'],
                FilterDateOfBirthStart: params.filter().FilterDateOfBirth.startDate,
                FilterDateOfBirthEnd: params.filter().FilterDateOfBirth.endDate,
                FilterAddedDateStart: params.filter().FilterAddedDate.startDate,
                FilterAddedDateEnd: params.filter().FilterAddedDate.endDate,
                FilterActivatedDateStart: params.filter().FilterActivatedDate.startDate,
                FilterActivatedDateEnd: params.filter().FilterActivatedDate.endDate,
                FilterLastVisitDateStart: params.filter().FilterLastVisitDate.startDate,
                FilterLastVisitDateEnd: params.filter().FilterLastVisitDate.endDate,
                FilterRoles: params.filter()['Roles']
            };
            var url = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/api/Table/Users';
            $http.get(url, { params: p }) //наш контроллер с методом для получания списка
            .success(function(data, status, headers, config) {
                params.total(data.totalItems);
                $defer.resolve(data.items);
            }).error(function(data, status, headers, config) {
                alert(JSON.stringify(data));
            });
        }
    });

    $scope.clearFilter = function () {
        $scope.tableParams.filter({});
        $scope.tableParams.filter().FilterDateOfBirth = { startDate: null, endDate: null };
        $scope.tableParams.filter().FilterAddedDate = { startDate: null, endDate: null };
        $scope.tableParams.filter().FilterActivatedDate = { startDate: null, endDate: null };
        $scope.tableParams.filter().FilterLastVisitDate = { startDate: null, endDate: null };
    };
    $scope.editId = -1;

    $scope.setEditId =  function(user) {
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/api/Table/Roles';
        $http.get(url) //наш контроллер с методом для получания списка
            .success(function(data, status, headers, config) {
                $scope.roles = data;
                angular.forEach(user.Roles, function(role) {
                    $scope.roles.splice(role, 1);
                });
                $scope.editId = user.ID;
            }).error(function(data, status, headers, config) {
                alert(JSON.stringify(data));
            });
    }


    $scope.save = function (user) {
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/api/Table/Users';
        $http.post(url, user) //наш контроллер с методом для получания списка
        .success(function(data, status, headers, config) {
            $scope.editId = -1;
        }).error(function (data, status, headers, config) {
            alert(JSON.stringify(data));
        });


    };

});