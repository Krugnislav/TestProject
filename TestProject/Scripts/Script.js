var app = angular.module('main', ['ngRoute', 'ngTable']).
controller('DemoCtrl', function ($scope, $http, $filter, ngTableParams) {

    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 10,          // count per page
        filter: {
            name: 'M'       // initial filter
        },
        sorting: {
            name: 'asc'     // initial sorting
        }
    }, {

        getData: function ($defer, params) {
            var p = {
                sortOrder: params.sorting
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