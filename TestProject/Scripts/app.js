var app = angular.module('MyApp', ['ngGrid']);

app.controller('MainController', ['$scope', '$http', function ($scope, $http, $apply) {

    // filter
    $scope.filterOptions = {
        filterText: "",
        useExternalFilter: true
    };

    // paging
    $scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [25, 50, 100],
        pageSize: 25,
        currentPage: 1
    };

    // sort
    $scope.sortOptions = {
        fields: ["name"],
        directions: ["ASC"]
    };

    // grid
    $scope.gridOptions = {
        data: "items",
        columnDefs: [
            { field: "ID", displayName: "ID", width: "60" },
            { field: "Name", displayName: "Name", pinnable: true },
            { field: "LastName", displayName: "LastName", width: "100" },
            { field: "Email", displayName: "Email", width: "100" },
            { field: "Roles", displayName: "Roles", width: "100" }
        ],
        enablePaging: true,
        enablePinning: true,
        enableCellEdit: true,
        pagingOptions: $scope.pagingOptions,
        filterOptions: $scope.filterOptions,
        keepLastSelected: true,
        multiSelect: false,
        showColumnMenu: true,
        showFilter: true,
        showFooter: true,
        sortInfo: $scope.sortOptions,
        totalServerItems: "totalServerItems",
        useExternalSorting: true,
        i18n: "ru"
    };

    $scope.refresh = function () {
        setTimeout(function () {
            $http({
                url: "admin/api/Table",
                method: "GET",
            }).success(function (data, status, headers, config) {
                $scope.totalServerItems = data.totalItems;
                $scope.items = data.items;
            }).error(function (data, status, headers, config) {
                alert(JSON.stringify(data));
            });
        }, 100);
    };

    // watches
    $scope.$watch('pagingOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.refresh();
        }
    }, true);

    $scope.$watch('filterOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.refresh();
        }
    }, true);

    $scope.$watch('sortOptions', function (newVal, oldVal) {
        if (newVal !== oldVal) {
            $scope.refresh();
        }
    }, true);

    $scope.refresh();
}]);