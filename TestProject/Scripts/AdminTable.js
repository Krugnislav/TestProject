var app = angular.module('main', ['ngRoute', 'daterangepicker', 'ngTable', 'ui.bootstrap']).
controller('DemoCtrl', function ($scope, $http, $filter, $location, $modal, ngTableParams) {

    $scope.roles = [];
    $scope.role = '';

    $scope.RoleFilter = [];

    $scope.statuses = ['Активирован', 'Дезактивирован'];

    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 10,          // count per page
        filter: {
            FilterDateOfBirth : { startDate: null, endDate: null },
            FilterAddedDate: { startDate: null, endDate: null },
            FilterActivatedDate: { startDate: null, endDate: null },
            FilterLastVisitDate: { startDate: null, endDate: null },
            Role: { Name: null }
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
                FilterStatus: params.filter().status,
                FilterRoles: params.filter().Role.Name
            };
            var url = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/api/Table/Users';
            $http.get(url, { params: p }) //наш контроллер с методом для получания списка
            .success(function(data, status, headers, config) {
                params.total(data.totalItems);
                $defer.resolve(data.items);
                if ($scope.RoleFilter.length < 2) {
                    angular.forEach(data.roles, function (role) {
                        var item = role;
                        item.isUser = false;
                        $scope.RoleFilter.push(item);
                    });
                }
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
        $scope.tableParams.filter().Role = { name: null};
    };

    $scope.open = function (size, user) {
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/Users/Edit';
        var modalInstance = $modal.open({
            templateUrl: url,
            controller: 'ModalInstanceCtrl',
            size: size,
            resolve: {
                item: function () {
                    return angular.copy(user);
                },
                statuses: function () {
                    return $scope.statuses;
                },
                roles: function () {
                    angular.forEach($scope.RoleFilter, function (role) {
                        role.isUser = false;
                        angular.forEach(user.Roles, function (roleU) {
                            if (role.ID == roleU.ID) role.isUser = true;
                        });
                    });
                    return $scope.RoleFilter;
                }
            }
        });

        modalInstance.result.then(function (form) {
            user.Name = form.Name;
            user.LastName = form.LastName;
            user.DateOfBirth = form.DateOfBirth;
            user.Roles = form.Roles;
            user.Status = form.Status;
        }, function () {
        });
    };

    $scope.create = function (size) {
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/Users/Create';
        var modalInstance = $modal.open({
            templateUrl: url,
            controller: 'ModalCreateCtrl',
            size: size,
            resolve: {
            }
        });

        modalInstance.result.then(function (form) {

        }, function () {
        });
    };

});

app.controller('ModalInstanceCtrl', function ($scope, $modalInstance, $location, $http, item, statuses, roles) {

    $scope.userEdit = item;

    $scope.statuses = statuses;

    $scope.roles = roles;

    $scope.change = function (role) {
        if (role.isUser) {
            $scope.role = { ID: '', Code: '', Name: '' };
            $scope.role.ID = role.ID;
            $scope.role.Code = role.Code;
            $scope.role.Name = role.Name;
            $scope.userEdit.Roles.push(role);
        } else {
            $scope.userEdit.Roles.splice($scope.userEdit.Roles.indexOf(role), 1);
        }
    };

    $scope.ok = function () {
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/api/Table/Users';
        $http.post(url, $scope.userEdit) //наш контроллер с методом для получания списка
        .success(function () {
            $modalInstance.close($scope.userEdit);
        }).error(function (error) {
            $scope.validationErrors = [];
            if (error && angular.isObject(error.ModelState)) {
                for (var key in error.ModelState) {
                    $scope.validationErrors.push(error.ModelState[key][0]);
                }
            } else {
                $scope.validationErrors.push('Could not add movie.');
            };
        });
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.disabled = function (date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };


});

app.controller('ModalCreateCtrl', function ($scope, $modalInstance, $location, $http) {


    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
    $scope.form = {};

    $scope.create = function () {
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/Users/Create';
        $http.post(url, $scope.form).success(function (data, status, headers, config) {
            if (data.res.length == 2) {
            } else {
            }
        }).error(function (err) {
            alert(err);
        });
        $modalInstance.close();
    };

    $scope.disabled = function (date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };
    $scope.maxdate = ['01.02.2001'];
    $scope.open = function ($event) {
        $scope.maxdate = new Date();
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };
    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };


});