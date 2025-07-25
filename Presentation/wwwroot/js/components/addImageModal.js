$('#addImageModal').on('show.bs.modal', function (event) {
    const button = $(event.relatedTarget)
    const id = button.data('id')
    let modalBody = $(".add-image-modal-body")
    modalBody.find("[name=ModelId]").val(id)

})
