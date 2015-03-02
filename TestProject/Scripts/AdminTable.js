var app = angular.module('main', ['ngRoute', 'daterangepicker', 'ngTable', 'mgcrea.ngStrap', 'ngSanitize',]);

app.controller('DemoCtrl', function ($scope, $http, $filter, $location, $modal, ngTableParams) {

    $scope.roles = [];
    $scope.role = '';
    $scope.urlEditModal = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/Users/Edit';
    $scope.urlCreateModal = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/Users/Create';

    var editModal = $modal({ scope: $scope, template: $scope.urlEditModal, show: false });

    $scope.showEditModal = function () {
        editModal.$promise.then(editModal.show);
    };

    var createModal = $modal({ scope: $scope, template: $scope.urlCreateModal, show: false });

    $scope.showCreateModal = function () {
        createModal.$promise.then(createModal.show);
    };

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

    $scope.open = function (user) {
        $scope.user = user;
        angular.forEach($scope.RoleFilter, function (role) {
            role.isUser = false;
            angular.forEach(user.Roles, function (roleU) {
                if (role.ID == roleU.ID) role.isUser = true;
            });
        });
        editModal.$promise.then(editModal.show);
    };

    $scope.create = function (size) {
        createModal.$promise.then(createModal.show);
    };

});

app.config(function ($datepickerProvider) {
    angular.extend($datepickerProvider.defaults, {
        dateFormat: 'dd.MM.yyyy',
        startWeek: 1
    });
})


app.controller('ModalEditCtrl', function ($scope, $location, $http) {

    $scope.userEdit = angular.copy($scope.user);


    $scope.roles = $scope.RoleFilter;

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
    $scope.maxdate = new Date();

    $scope.ok = function () {
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/api/Table/Users';
        $http.post(url, $scope.userEdit) //наш контроллер с методом для получания списка
        .success(function () {
            $scope.user.Name = $scope.userEdit.Name;
            $scope.user.LastName = $scope.userEdit.LastName;
            $scope.user.DateOfBirth = $scope.userEdit.DateOfBirth;
            $scope.user.Roles = $scope.userEdit.Roles;
            $scope.user.Status = $scope.userEdit.Status;
            $scope.$hide();
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

});

app.controller('ModalCreateCtrl', function ($scope, $location, $http) {


    $scope.form = {};

    $scope.create = function () {
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port() + '/Admin/Users/Create';
        $http.post(url, $scope.form).success(function (data, status, headers, config) {
            if (data.res.length == 2) {
                $scope.$hide();
            } else {
            }
        }).error(function (err) {
            alert(err);
        });
    };

    $scope.maxdate = new Date();


});