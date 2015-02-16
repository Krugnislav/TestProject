var uid = 1;


function ContactController($scope, $http) {

    $scope.isSearching = false;
    $scope.radioModel = '5';
    $scope.pagingInfo = {
        page: 1,
        itemsPerPage: 5,
        sortBy: 'Name',
        reverse: false,
        filterCountry: '',
        filterCity: '',
        filterStreet: '',
        filterNumber: '',
        filterPostcode: '',
        filterDate: '',
        totalItems: 0
    };
    $scope.columncolor = 'even';

    $scope.tableHeaders = [{ color: "lc", sort: '', arrow: '', filter: '' },
                           { color: "lc", sort: '', arrow: '', filter: '' },
                           { color: "lc", sort: '', arrow: '', filter: '' },
                           { color: "lc", sort: '', arrow: '', filter: '' },
                           { color: "lc", sort: '', arrow: '', filter: '' },
                           { color: "lc", sort: '', arrow: '', filter: '' }]
    $scope.filterResult = function (key) {
        if ($scope.tableHeaders[key].filter === '') {
            if ($scope.tableHeaders[key].arrow === 'column-arrow') {
            } else {
                $scope.tableHeaders[key].color = 'lc';
            }
        } else {
            $scope.tableHeaders[key].color = 'even';
        }
        $scope.refresh();
    };
    $scope.sort = function (sortBy, key) {
        for (var i = $scope.tableHeaders.length - 1; i >= 0; i--) {
            if ($scope.tableHeaders[i].filter === '')
                $scope.tableHeaders[i].color = 'lc';
            $scope.tableHeaders[i].sort = '';
            $scope.tableHeaders[i].arrow = '';
        }
        $scope.tableHeaders[key].color = 'even';
        $scope.tableHeaders[key].arrow = 'column-arrow';
        if (sortBy === $scope.pagingInfo.sortBy) {
            $scope.pagingInfo.reverse = !$scope.pagingInfo.reverse;
        } else {
            $scope.pagingInfo.sortBy = sortBy;
            $scope.pagingInfo.reverse = false;
        }
        if ($scope.pagingInfo.reverse) {
            $scope.tableHeaders[key].sort = 'table-sort-asc';
        } else {
            $scope.tableHeaders[key].sort = 'table-sort-desc';
        }
        $scope.pagingInfo.page = 1;
        $scope.refresh();
    };
    $scope.itemsPerPageChange = function () {
        $scope.refresh();
    }
    $scope.search = function () {
        $scope.pagingInfo.page = 1;
        $scope.refresh();
    };
    $scope.pageChanged = function () {
        $scope.refresh();
    };
    $scope.refresh = function () {
        $scope.pagingInfo.filterCountry = $scope.tableHeaders[0].filter;
        $scope.pagingInfo.filterCity = $scope.tableHeaders[1].filter;
        $scope.pagingInfo.filterStreet = $scope.tableHeaders[2].filter;
        $scope.pagingInfo.filterNumber = $scope.tableHeaders[3].filter;
        $scope.pagingInfo.filterPostcode = $scope.tableHeaders[4].filter;
        $scope.pagingInfo.filterDate = $scope.tableHeaders[5].filter;
        $http.get('api/Item', { params: $scope.pagingInfo }) //наш контроллер с методом для получания списка
            .success(function (data, status, headers, config) {
                console.log('Got keyup event.');
                $scope.pagingInfo.totalItems = data.totalItems;
                console.log($scope.pagingInfo.totalItems);
                $scope.houses = data.houses;
                $scope.isSearching = false;
            }).error(function (data, status, headers, config) {
                $scope.isSearching = false;
                alert(JSON.stringify(data));
            });
        $scope.isSearching = true;
    }
    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };
    $scope.today = function () {
        $scope.dt = new Date();
    };
    $scope.today();

    $scope.clear = function () {
        $scope.dt = null;
    };

    // Disable weekend selection
    $scope.disabled = function (date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };
    $scope.toggleMin();

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };
    $scope.refresh();
}
ContactController.$inject = ['$scope', '$http'];