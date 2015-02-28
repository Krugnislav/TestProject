var app2 = angular.module('valid', ['ngRoute', 'mgcrea.ngStrap', 'angularFileUpload', 'ngSanitize', 'ngImgCrop']);

app2.controller('index', function ($scope, $http, $location, $window) {

            // Injector
            $scope.user = {};

            $scope.create = function () {
                var url = $location.protocol() + '://' + $location.host() + ':' + $location.port();
                $http.post(url + '/api/UserEdit/Create', $scope.user).success(function () {
                        $window.location.href = url + '/User/GoodCreate' ;
                    }).error(function (err) {
                        alert(JSON.stringify(err));
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

        });

app2.controller('PopoverDemoCtrl', function ($scope, $popover) {
    $scope.popover = {
        "title": "Title",
        "content": "Hello Popover<br />This is a multiline message!"
    };

});

app2.config(function($popoverProvider) {
    angular.extend($popoverProvider.defaults, {
        html: true
    });
})

app2.controller('UserEditController', function ($scope, $http, $location, $window, user) {

    // Injector
    $scope.user = user;
    $scope.user.avatarPath = $location.protocol() + '://' + $location.host() + ':' + $location.port() + user.avatarPath;

    $scope.edit = function () {
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port();
        $http.post(url + '/api/UserEdit/Edit', $scope.user).success(function () {
            $window.location.href = url + '/Home' ;
        }).error(function (err) {
            alert(JSON.stringify(err));
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

    $scope.modal = {
        "title": "Title",
        "content": "Hello Modal<br />This is a multiline message!"
    };


});

app2.controller('EditAvatarController', ['$scope', '$upload', function ($scope, $upload) {
    $scope.myImage = '';
    $scope.myCroppedImage = '';

    var handleFileSelect = function (evt) {
        var file = evt.currentTarget.files[0];
        var reader = new FileReader();
        reader.onload = function (evt) {
            $scope.$apply(function ($scope) {
                $scope.myImage = evt.target.result;
            });
        };
        reader.readAsDataURL(file);
    };

    angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);



}]);
