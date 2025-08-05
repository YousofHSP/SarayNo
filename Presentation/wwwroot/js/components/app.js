$(".changeType").click(function () {
    let id = $(this).data("id");
    let type = $(this).data("type");
    let data = {type: type};
    $.ajax({
        url: `/Admin/api/Invoice/${id}/ChangeType?type=${type}`,
        method: "get",
        success: result => {
            Swal.fire({
                icon: "success",
                title: "ثبت شد",
                text: "اطلاعات با موفقیت ثبت شد"
            }).then(res => location.reload());
        }
    })
})
$(".changeStatus").click(function () {
    let id = $(this).data("id");
    let status = $(this).data("status");
    let modalBody = $(".change-status-modal-body");
    modalBody.find("[name=Id]").val(id);
    modalBody.find("[name=Status]").val(status);
    $("#changeStatusModal").modal("show");
})
$("#changeStatusBtn").click(function () {

    let modalBody = $(".change-status-modal-body");
    let id = modalBody.find("[name=Id]").val();
    let status = Number(modalBody.find("[name=Status]").val());
    let description = modalBody.find("[name=Description]").val();
    let data = {status, id, description};
    $.ajax({
        url: `/Admin/api/Invoice/ChangeStatus`,
        contentType: 'application/json',
        data: JSON.stringify(data),
        method: "post",
        success: () => {
            Swal.fire({
                icon: "success",
                title: "ثبت شد",
                text: "اطلاعات با موفقیت ثبت شد"
            }).then(() => location.reload());
        }
    })
})
$("#addPayoff").click(function () {
    let form = $("#addPayoffForm");
    let invoiceId = form.find("[name=InvoiceId]").val();
    let type = form.find("[name=type]").val();
    let description = form.find("[name=description]").val();
    let date = form.find("[name=date]").val();
    let dueDate = form.find("[name=dueDate]").val();
    let price = form.find("[name=price]").val();
    let data = {invoiceId, type, description, date, price, dueDate};
    const modal = $("#detailModal")
    $.ajax({
        url: `/Admin/api/Invoice/AddPayoff`,
        contentType: 'application/json',
        data: JSON.stringify(data),
        method: "post",
        success: (res) => {
            let remainStr = modal.find("#remain-amount-el").text()
            let remain = parseInt(remainStr.replace(/,/g, ''));
            let price = parseInt(res.price.replace(/,/g, ''));
            
            remain -= price;
            modal.find("#remain-amount-el").text(remain.toLocaleString("en-US"))
            let payoffsEl = `
            <tr>
                <td>${res.typeDisplay}</td>
                <td>${res.description}</td>
                <td>${res.date}</td>
                <td>${res.dueDate}</td>
                <td colspan="2">${res.price}</td>
            </tr>
        `
            $('#payoffs-table').append(payoffsEl)
            Swal.fire({
                icon: "success",
                title: "ثبت شد",
                text: "اطلاعات با موفقیت ثبت شد"
            })

        }
    })
})
$("body").on("click", ".deletePayoff", function(){
    const id = $(this).data("id");
    const trEl = $(this).parents("tr")
    $.ajax({
        url: `/Admin/api/Invoice/DeletePayoff/${id}`,
        contentType: 'application/json',
        method: "post",
        success: () => {
            trEl.remove();
        },
        error: (xhr) => {
            console.log(xhr)
            Swal.fire({
                icon: "error",
                title: "خطا در حذف",
                text: xhr.responseJSON?.message || "خطایی رخ داده است."
            })
            
        }
    })
})