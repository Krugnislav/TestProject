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
            $scope.maxdate = new Date();

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
app2.config(function ($datepickerProvider) {
    angular.extend($datepickerProvider.defaults, {
        dateFormat: 'dd.MM.yyyy',
        startWeek: 1
    });
})

app2.controller('UserEditController', function ($scope, $http, $location, $window, user) {

    $scope.show = false;
    $scope.user = user;
    $scope.user.avatarPath = $location.protocol() + '://' + $location.host() + ':' + $location.port() + user.avatarPath.replace('~', '');

    $scope.edit = function () {
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port();
        $http.post(url + '/api/UserEdit/Edit', $scope.user).success(function () {
            $window.location.href = url + '/Home' ;
        }).error(function (err) {
            alert(JSON.stringify(err));
        });
                
    };
    $scope.hidePass = function () {
        $scope.show = false;
    }

    $scope.showPass = function () {
        $scope.show = true;
    }

    $scope.editPass = function() {
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port();
        var p = {
            id: user.id,
            password: user.password,
            newpassword: user.newpassword
        };
        $http.post(url + '/api/UserEdit/EditPassword', p).success(function () {
            $scope.show = false;
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
        "title": "Edit avatar",
        "content": "Hello Modal<br />This is a multiline message!"
    };


});

app2.controller('EditAvatarController', function ($scope, $timeout, $location, $http) {

    $scope.myImage='';
    $scope.myCroppedImage='';

    var handleFileSelect=function(evt) {
        $scope.chek='Here!';
      var file=evt.currentTarget.files[0];
      var reader = new FileReader();
      reader.onload = function (evt) {
        $scope.$apply(function($scope){
        $scope.chek2='And Here!';
          $scope.myImage=evt.target.result;
        });
      };
      reader.readAsDataURL(file);
    };
    $timeout(function () {
        angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);

    }/* no delay here */);

    $scope.save = function () {
        $scope.user.avatarPath = $scope.myCroppedImage;
        var url = $location.protocol() + '://' + $location.host() + ':' + $location.port();
        var data = $scope.myCroppedImage.replace('data:image/png;base64,', '');
        $http.post(url + '/api/UserEdit/EditAvatar', {id: $scope.user.id, imageData: data}).success(function () {
            $scope.$hide();
        }).error(function (err) {
            alert(JSON.stringify(err));
        });
    }
});
