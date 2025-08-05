$('#addImageModal').on('show.bs.modal', function (event) {
    const button = $(event.relatedTarget)
    const id = button.data('id')
    let modalBody = $(".add-image-modal-body")
    let returnUrl = document.referrer;
    console.log(returnUrl)
    modalBody.find("[name=ModelId]").val(id)
    modalBody.find("[name=ReturnUrl]").val(returnUrl)

})
