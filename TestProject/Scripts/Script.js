var app = angular.module('main', ['ngRoute', 'ngBootstrap', 'ngTable']).
controller('DemoCtrl', function ($scope, $http, $filter, ngTableParams) {
    $scope.myDateRange;
    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 10,          // count per page
        filter: {
            Name: ''       // initial filter
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
            $scope.myDateRange = $filter('date')(params.filter()['DateOfBirth']['startDate'], "dd.MM.yyyy") + '-' + $filter('date')(params.filter()['DateOfBirth']['endDate'], "dd.MM.yyyy");
            var p = {
                PageNumber: $scope.tableParams.page(),
                PageSize: $scope.tableParams.count(),
                SortColumn: $scope.SortColumn,
                SortOrder: $scope.SortOrder,
                FilterName: params.filter()['Name'],
                FilterID: params.filter()['ID'],
                FilterEmail: params.filter()['Email'],
                FilterLastName: params.filter()['LastName'],
                FilterDateOfBirth: params.filter()['DateOfBirth']['startDate'] + '-' + params.filter()['DateOfBirth']['endDate'],
                FilterAddedDate: params.filter()['AddedDate'],
                FilterActivatedDate: params.filter()['ActivatedDate'],
                FilterLastVisitDate: params.filter()['LastVisitDate'],
                FilterRoles: params.filter()['Roles']
        };
            $http.get('admin/api/Table', { params: p }) //наш контроллер с методом для получания списка
            .success(function(data, status, headers, config) {
                params.total(data.totalItems);
                $defer.resolve(data.items);
            }).error(function(data, status, headers, config) {
                alert(JSON.stringify(data));
            });
        }
    });

});