(function () {
    angular.module('valid', ['ui.bootstrap'])

        .controller('index', ['$scope', '$http', function ($scope, $http) {

            // Injector
            $scope.form = {};

            $scope.create = function () {
                    $http.post('Create', $scope.form).success(function (res) {
                        if (res.data == 'ok') {
                            $scope.user = {};
                            $scope.form.$setPristine();
                            alert('Сообщение отправлено');
                            location.path('~/Home/Index');
                        } else {
                            alert('Возникла ошибка');
                        }
                        location.path('~/Home/Index');

                    }).error(function (err) {
                        alert(err);
                    });
                
            };
            $scope.maxdate = ['01.02.2001'];

            $scope.disabled = function (date, mode) {
                return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
            };

            $scope.open = function ($event) {
                $scope.maxdate = new Date();
                $event.preventDefault();
                $event.stopPropagation();

                $scope.opened = true;
            };

        }]);
}).call(this);


