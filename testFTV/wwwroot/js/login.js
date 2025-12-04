$(function () {
  $(".disable-enter").keydown(function (event) {
    if (event.keyCode === 13) {
      event.preventDefault();
    }
  });

  const togglePassword = (selector) => {
    const input = $(selector);
    input.toggleClass('fa-eye fa-eye-slash');
    const inputType = input.siblings('input').attr("type");
    input.siblings('input').attr("type", inputType === "password" ? "text" : "password");
  };

  const eyeElements = [
    ".login .checkEye",
    ".change-password .old-pass .eye",
    ".change-password .new-pass .eye",
    ".change-password .check-newPass .eye",
    ".setting-new-password .setting-pass .eye",
    ".setting-new-password .check-pass .eye",
    ".register .setting-pass .eye",
    ".register .check-pass .eye"
  ];

  for (const eyeElement of eyeElements) {
    $(eyeElement).click(function () {
      togglePassword(this);
    });
  }

  $(".burger_menu").click(function () {
    $("#menu").slideToggle("slow", function () {
      const isOpen = $("#menu").css("display") === "block";
      $(".burger_menu").toggleClass("fa-xmark", isOpen).toggleClass("fa-bars", !isOpen);
    });
  });

  $(".copy").click(function () {
    let ID = $(".invitation-code")[0]
    navigator.clipboard.writeText(ID.innerText);
    alert("複製成功：" + ID.innerText);
  })

  $(".goBack").click(function () {
    history.go(-1);
  })

});

/*驗證密碼格式是否正確*/
$(function () {
  const $passwordInput = $("#REPW");
  const $checkPasswordInput = $("#RECheckPW");
  const $errorMessage = $("#pw-error-message");
  const $submitButton = $("#Button2");

  openBtn();

  function openBtn() {
    $submitButton.prop("disabled", true).css("background-color", "#e7e7e7");
  }
  function closeBtn() {
    $submitButton.prop("disabled", false).css("background-color", "");
  }

  function displayError(message) {
    $errorMessage.css({
      display: 'block',
      color: '#f76565',
      'font-weight': 'bold',
      'font-size': '12px'
    }).html(`<p>${message}</p>`);

    $checkPasswordInput.css({
      border: '2px solid #f76565',
      outline: 'none',
      background: '#FFF'
    }).removeClass('form-control');
  }

  function hideError() {
    $errorMessage.css('display', 'none');
    $checkPasswordInput.css('border', '1px solid #ccc');
  }

  function checkPasswordMatch() {
    const passwordValue = $passwordInput.val().trim()
    const checkPasswordValue = $checkPasswordInput.val().trim();
    const regexPasswordPattern = /^[a-zA-Z0-9]{8,}$/;
    if (passwordValue === checkPasswordValue && regexPasswordPattern.test(passwordValue)) {
      hideError();
      closeBtn()
    } else {
      displayError('密碼不一致或不符合格式要求');
      openBtn()
    };
  }

  $checkPasswordInput.on('input', function () {
    checkPasswordMatch();
  });

  $passwordInput.on('input', function () {
    checkPasswordMatch();
  });
});


const getUserAc = localStorage.getItem("UserAc");
let modalVisible = false;
let time = 60;
let interval;
const mainBtn = $(".main_btn");

/*驗證註冊帳號格式是否正確*/
$(function () {
  const $accountInput = $("#REAccount");
  const $errorMessage = $("#AccountErrorMessage");
  const $submitButton = $("#Button1");
  // 頁面加載時檢查輸入框是否為空
  checkInputValidity();
  // 監聽輸入框的輸入事件，以便在用戶輸入時動態檢查輸入框是否為空
  $accountInput.on("input", function () {
    checkInputValidity();
  });

  function checkInputValidity() {
    if ($accountInput.length > 0) {
      let accountText = $accountInput.val().trim();
      if (accountText === "") {
        // 輸入框為空，禁用按鈕
        $submitButton.prop("disabled", true).css("background-color", "#e7e7e7");
      } else {
        // 輸入框不為空，啟用按鈕
        $submitButton.prop("disabled", false).css("background-color", "");
      }
    }
  }
  function displayError(message) {
    $errorMessage.css({
      display: 'block',
      color: '#f76565',
      'font-weight': 'bold',
      'font-size': '12px'
    }).html(`<p>${message}</p>`);

    $accountInput.css({
      border: '2px solid #f76565',
      outline: 'none',
      background: '#FFF'
    }).removeClass('form-control');
    $submitButton.prop("disabled", true).css("background-color", "#e7e7e7");
  }

  function hideError() {
    $errorMessage.css('display', 'none');
    $accountInput.css('border', '1px solid #ccc');
    $submitButton.prop("disabled", false).css("background-color", "");
  }

  $accountInput.on('input', function () {
    const inputValue = $accountInput.val().trim();
    const RegexPhonePattern = /^[0][9][0-9]{8}$/;
    const RegexMailPattern = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z]+$/;
    if (RegexPhonePattern.test(inputValue) || RegexMailPattern.test(inputValue)) {
      hideError();
    } else {
      displayError('帳號格式有誤');
    }
  });
})

//驗證碼
function sendCode() {
  const btn = $("#Button1");
  const accountText = $("#REAccount").val().trim();
  const url = `Register.aspx?A=${accountText}`
  showLoading();
  sendRequest("POST", url, null, "json", function (response) {
    hideLoading();
    if (response.Status === "Success") {
      interval = setInterval(function () {
        time--;
        if (time == 0) {
          clearInterval(interval);
          btn.css("background-color", "");
          btn.prop("disabled", false);
          btn.val("發送驗證碼");
          return;
        } else {
          btn.css("background-color", "#e7e7e7");
          btn.prop("disabled", true);
          btn.val("重新發送(" + time + "s)");
        }
      }, 1000);
    } else {
      showMessageModal(response.Desc);
    }
  },
    function (error) {
      console.error("失敗:" + error)
    }
  )
  return false
}

//註冊
function submitMember() {
  const accountText = $("#REAccount").val().trim();
  const verifiText = $("#REVerifi").val().trim();
  const nameText = $("#REName").val().trim();
  const pwText = $("#REPW").val().trim();
  const url = `Register.aspx?B=${accountText}&C=${pwText}&D=${nameText}&E=${verifiText}`;
  sendRequest("POST", url, null, "json", function (response) {
    if (response.Status === "Success") {
      setModalState(true, "註冊成功", true);
      mainBtn.off("click").click(function () {
        handleModalDisplay(false);
        window.location.replace("login.aspx");
      });
    } else {
      showMessageModal(response.Desc);
    }
  },
    function (error) {
      console.error("失敗:" + error)
    }
  )
  return false
}

//會員登入
function Login() {
  const accountText = $("#LoginAccount").val().trim();
  const pwText = $("#LoginPassword").val().trim();
  let stringRead = {
    fsAccountID: accountText,
    fdPassword: pwText,
  };
  const url = "Login.aspx";
  sendRequest("POST", url, stringRead, "json", function (response) {
    if (response.Status === "Success") {
      setCookie("FTVGuid", response.guid, 30)
      window.location.replace("invite.aspx");
    } else {
      let errorMessage = response.Desc;
      showMessageModal(errorMessage);
    }
  },
    function (error) {
      console.error("失敗:" + error)
    }
  )
  return false
}

//忘記密碼
function submitForgotCode() {
  const accountText = $("#ForgotAccount").val().trim();
  let stringRead = {
    fsAccountID: accountText,
  };
  showLoading();
  const url = "Forgot_Password.aspx";
  sendRequest("POST", url, stringRead, "json", function (response) {
    if (response.Status === "Success") {
      setModalState(true, "發送成功", true);
      localStorage.setItem("UserAc", accountText);
      mainBtn.off("click").click(function () {
        handleModalDisplay(false);
        window.location.replace("Vertify_Code.aspx");
      });
    } else {
      const errorMessage = accountText === "" ? "請填寫帳號" : response.Desc;
      showMessageModal(errorMessage);
    }
  },
    function (error) {
      console.error("失敗:" + error)
    }
  )
  return false
}

//忘記密碼驗證碼
function resentMessage() {
  if (!modalVisible) {
    modalVisible = true;
    if (!interval) {
      showLoading();
      const url = `Vertify_Code.aspx?A=${getUserAc}`;
      sendRequest("POST", url, null, "json", function (response) {
        hideLoading();
        if (response.Status === "Success") {
          interval = setInterval(function () {
            time--;
            if (time === 0) {
              clearInterval(interval);
              interval = null;
              modalVisible = false;
              handleModalDisplay(false);
              return;
            } else {
              setModalState(true, "發送成功<br/>請於" + time + "秒後重試", modalVisible);
            }
          }, 1000)
        } else {
          showMessageModal(response.Desc);
        }
      },
        function (error) {
          console.error("失敗:" + error)
        }
      )
    }
  }
  return false
}

//提交忘記密碼驗証
function resentSubmit() {
  const fieldList = document.querySelector('.field-list');
  const fieldControls = fieldList.querySelectorAll('.form-control');
  let combinedValue = '';
  fieldControls.forEach(control => {
    combinedValue += control.textContent.trim();
  });
  const url = `Vertify_Code.aspx?B=${getUserAc}&C=${combinedValue}`;
  sendRequest("POST", url, null, "json", function (response) {
    if (response.Status === "Success") {
      window.location.replace("Setting_NewPassword.aspx");
    } else {
      const errorMessage = combinedValue === "" ? "請輸入驗證碼" : response.Desc;
      showMessageModal(errorMessage);
    }
  },
    function (error) {
      console.error("失敗:" + error)
    }
  )
  return false
}

//設定新密碼
function settingNewPw() {
  const passwordInput = $("#REPW").val().trim();
  const checkPasswordInput = $("#RECheckPW").val().trim();
  const url = "Setting_NewPassword.aspx";
  let stringRead = {
    fsAccountID: getUserAc,
    fdPassword: passwordInput,
    checkPasswordInput: checkPasswordInput
  };
  showLoading();
  sendRequest("POST", url, stringRead, "json", function (response) {
    hideLoading();
    if (response.Status === "Success") {
      setModalState(true, "設定成功", true);
      mainBtn.off("click").click(function () {
        handleModalDisplay(false);
        console.log("發送成功:" + response.Status)
        window.location.replace("login.aspx");
      });
    } else {
      showMessageModal(response.Desc);
    }
  },
    function (error) {
      console.error("失敗:" + error)
      showMessageModal("密碼格式錯誤");
      hideLoading();
    })
  return false
}

//修改密碼
function changePW() {
  const OldPass = $("#OldPass").val().trim();
  const passwordInput = $("#REPW").val().trim();
  const checkPasswordInput = $("#RECheckPW").val().trim();
  const url = "Change_Password.aspx";
  let stringRead = {
    fdPasswordOld: OldPass,
    fdPasswordNew: passwordInput,
    fdPasswordCheckNew: checkPasswordInput
  }
  showLoading();
  sendRequest("POST", url, stringRead, "json", function (response) {
    hideLoading();
    if (response.Status === "Success") {
      setModalState(true, "密碼修改成功", true);
      mainBtn.off("click").click(function () {
        handleModalDisplay(false);
        console.log("發送成功:" + response.Status)
        window.location.replace("invite.aspx");
      });
    } else {
      showMessageModal(response.Desc);
    }
  },
    function (error) {
      console.error("失敗:" + error);
      showMessageModal("密碼格式錯誤");
      hideLoading();
    })
  return false
};

//刪除帳號
function AccountDel() {
  const subBtn = $(".sub_btn");
  const url = `User_Setting.aspx`;
  let setModal = handleModalDisplay(true);
  if (setModal) {
    mainBtn.off("click").click(function () {
      sendRequest("POST", url, null, "json", function (response) {
        if (response.Status === "Success") {
          console.log("成功:" + response.Status);
          handleModalDisplay(false);
          window.location.replace("login.aspx");
        }
      },
        function (error) {
          console.error("失敗:" + error);
        })
    });
    subBtn.off("click").click(function () {
      handleModalDisplay(false);
    })
  }
}

$(function () {
  if (getUserAc) {
    $("#AC").text(getUserAc);
  }
})

function checkBtn() {
  modalVisible = false;
  handleModalDisplay(false);
}

//Google與Apple快速登入
(function () {
  let firebaseConfig = {
    apiKey: "AIzaSyDXi6p_32P12e3ZMcIwE8ekGcnwrr43kWk",
    authDomain: "ftvnewsapp-df4b2.firebaseapp.com",
    databaseURL: "https://ftvnewsapp-df4b2.firebaseio.com",
    projectId: "ftvnewsapp-df4b2",
    storageBucket: "ftvnewsapp-df4b2.appspot.com",
    messagingSenderId: "234178802402",
    appId: "1:234178802402:web:fe633a5fb0a717663264bb",
  };

  if (window.location.href.toLowerCase().includes('login.aspx')) {
    firebase.initializeApp(firebaseConfig);
    const GoogleAut = new firebase.auth.GoogleAuthProvider();
    const AppleAut = new firebase.auth.OAuthProvider('apple.com');

    function signInWithProvider(type, provider) {
      return firebase.auth().signInWithPopup(provider)
        .then((result) => {
          const user = result.user.providerData[0];       
          const id = user.uid;
          const name = user.displayName;
          const email = user.email;
          const picture = user.photoURL;
          fastSignIn(type, id, name, email, picture)
        })
        .catch((error) => {
          console.log(`錯誤: ${error}`)
        });
    }
    $('#fbLogin').on('click', function () {
      FBLogin();
    });

    $('#AppleLogin').on('click', function () {
      signInWithProvider('apple', AppleAut);
    });
    $('#GLogin').on('click', function () {
      signInWithProvider('google', GoogleAut);
    });
  }

})();

//Line快速登入
$('#lineLogin').on('click', function () {
  let client_id = '2000035434';
  let redirect_uri = 'https://stage.ftvnews.com.tw/Login.aspx';
  let link = 'https://access.line.me/oauth2/v2.1/authorize?';
  link += 'response_type=code';
  link += '&client_id=' + client_id;
  link += '&redirect_uri=' + redirect_uri;
  link += '&state=login';
  link += '&scope=profile%20openid%20email';
  window.location.href = link;
});

const urlParams = new URLSearchParams(window.location.search);
if (urlParams.has('state') && urlParams.has('code')) {
  const code = urlParams.get('code');
  const state = urlParams.get('state');
  if (state === 'login') {
    const getTokenUrl = "https://api.line.me/oauth2/v2.1/token?";
    const getTokenBody = new URLSearchParams({
      grant_type: 'authorization_code',
      code,
      redirect_uri: 'https://stage.ftvnews.com.tw/Login.aspx',
      client_id: '2000035434',
      client_secret: 'f32e44110ee3aa35b2b996a9b33c6de9',
    });
    fetch(getTokenUrl, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      body: getTokenBody
    })
      .then(response => response.json())
      .then((el) => {
        //const access_token = el.access_token;
        const id_token = el.id_token;
        const getVerifyApiUrl = 'https://api.line.me/oauth2/v2.1/verify';
        const getVerifyBody = new URLSearchParams({
          client_id: '2000035434',
          id_token
        });
        fetch(getVerifyApiUrl, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
          },
          body: getVerifyBody
        })
          .then(response => response.json())
          .then((el2) => {
            const userId = el2.sub;
            const name = el2.name;
            const email = el2.email;
            const picture = el2.picture;
            fastSignIn("line", userId, name, email, picture)
          }).catch((error) => {
            console.log("錯誤:" + error)
          });
      }).catch((error) => {
        console.error("錯誤:" + error)
      })
  }
};

//處理登入取得的資訊
function fastSignIn(type, id, name, email, picture) {
  const url = `Login.aspx?A=${type}&B=${id}&C=${name}&D=${email}&E=${picture}`
  sendRequest("POST", url, null, "json", function (response) {
    if (response.Status === "Success") {
      setCookie("FTVGuid", response.guid, 30);
      window.location.replace("invite.aspx");
    }
  },
    function (error) {
      console.error("失敗:" + error.responseText)
    })
  return false;
};


//FB快速登入
function FBLogin() {
  FB.getLoginStatus(function (res) {
    if (res.status === "connected") {
      //用戶已授權您的App，用戶須先revoke撤除App後才能再重新授權你的App
      GetProfile();
    } else if (res.status === 'not_authorized' || res.status === "unknown") {
      //App未授權或用戶登出FB網站才讓用戶執行登入動作
      FB.login(function (response) {
        if (response.status === 'connected') {
          GetProfile();
        } else {
          alert("Facebook帳號無法登入");
        }
      }, { scope: 'email' });
    }
  }, true);
};

function GetProfile() {
  FB.api("/me", "GET", { fields: 'last_name,first_name,name,email,picture' }, function (user) {
    const userId = user.id;
    const name = user.name;
    const email = user.email;
    //const picture = user.picture.data
    fastSignIn("fb", userId, name, email, "")
  })
};

//上傳個人資料
function upload() {
  const name = $("#inputName").val().trim();
  const mail = $("#inputMail").val().trim();
  const phone = $("#inputTel").val().trim();
  const date = $("#inputDate").val().trim();
  const address = $("#inputAdd").val().trim();
  const code = $("#inputReco").val().trim();
  const selectedValue = $("#inputSex").val();
  const url = "Porfolie.aspx/Load_UserInfo";
  let stringRead = {
    fsName: name,
    fsEmail: mail,
    fsGender: selectedValue,
    fsBirthday: date,
    fsPhone: phone,
    fsAddress: address,
    fsRecommendCode: code
  }
  sendRequest("POST", url, stringRead, "json", function (response) {
    if (response.Status === "Success") {
      setModalState(true, "資料更新成功", true);
      mainBtn.off("click").click(function () {
        handleModalDisplay(false);
        console.log("發送成功:" + response.Status)
        window.location.replace("invite.aspx");
      });
    } else {
      showMessageModal(response.Desc);
    }
  },
    function (error) {
      console.error("錯誤:" + error.responseText)
    });
  return false;
};


