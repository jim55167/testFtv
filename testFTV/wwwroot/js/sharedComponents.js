//設置Modal狀態函數
function setModalState(isSuccess, iMessage, isVisible) {
  const icon = $("#staticBackdrop i.fa-duotone");
  const messageModal = $("#messageModal");
  const mainBtn = $(".main_btn");
  if (isSuccess) {
    icon.removeClass("fa-circle-xmark").addClass("fa-circle-check").css("color", "#358abc");
    if (iMessage != "") {
      messageModal.html(iMessage);
    }
  } else {
    icon.removeClass("fa-circle-check").addClass("fa-circle-xmark").css("color", "#BC3838");
    mainBtn.off("click").click(function () {
      handleModalDisplay(false);
    });
  }
  if (isVisible) {
    handleModalDisplay(true)
  }
}

function handleModalDisplay(flag) {
  const modal = $("#staticBackdrop");
  const modalBackdrop = $(".modal-backdrop.show");

  if (flag) {
    modal.addClass("show").css("display", "block");
    modalBackdrop.css("display", "block");
    return true;
  } else {
    modal.removeClass("show").css("display", "none");
    modalBackdrop.css("display", "none");
  }
}

function showMessageModal(message) {
  $("#messageModal").text(message);
  setModalState(false, "", true);
}

function sendRequest(requestType, url, data, dataType, successCallback, errorCallback) {
  $.ajax({
    type: requestType,
    url: url,
    data: data,
    dataType: dataType,
    success: successCallback,
    error: errorCallback
  });
}

function sendFileImage(url, formdata, dataType, successCallback, errorCallback) {
  $.ajax({
    type: "POST",
    url: url,
    data: formdata,
    contentType: false,
    processData: false,
    dataType: dataType,
    success: successCallback,
    error: errorCallback
  });
}

//dialog
function showLoading() {
  $(".dialog").css("display", "block");
}

function hideLoading() {
  $(".dialog").css("display", "none");
}

//存儲Cookie
function setCookie(name, value, days) {
  let expirationDate = null;
  if (days) {
    expirationDate = new Date();
    expirationDate.setDate(expirationDate.getDate() + days);
  }
  let cookieValue = `Bearer ${value}`;
  if (expirationDate) {
    cookieValue += `; expires=${expirationDate.toUTCString()}`;
  }
  document.cookie = `${name}=${cookieValue}; path=/`;
}

function deleteCookie(cookieName) {
  document.cookie = cookieName + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
  window.location.replace("login.aspx");
}