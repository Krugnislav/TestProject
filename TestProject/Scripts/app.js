(function () {
    angular.module('valid', ['ui.bootstrap'])

        .controller('index', ['$scope', '$http', function ($scope, $http) {

            // Injector

            $scope.form = {
                submit: function () {
                    $http.post('Create', $scope.form).success(function (res) {
                        if (res.data == 'ok') {
                            $scope.user = {};
                            $scope.form.$setPristine();
                            alert('Сообщение отправлено');
                        } else {
                            alert('Возникла ошибка');
                        }
                    }).error(function (err) {
                        alert(err);
                    });
                }
            };

            $scope.disabled = function (date, mode) {
                return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
            };

            $scope.open = function ($event) {
                $event.preventDefault();
                $event.stopPropagation();

                $scope.opened = true;
            };

        }]);
}).call(this);


