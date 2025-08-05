$('#detailModal').on('show.bs.modal', function (event) {
    const button = $(event.relatedTarget)
    const id = button.data('bs-id')
    const costGroup = button.data('bs-cost-group')
    const creditor = button.data('bs-creditor')
    const description = button.data('bs-description')
    const date = button.data('bs-date')
    const discount= button.data('bs-discount')
    const invoiceAmountStr= button.data('bs-invoice-amount')
    const modal = $(this)
    const modelFiles = files.filter(i => i.ModelId == id);
    const modelPayoffs = payoffs.filter(i => i.InvoiceId == id);
    let payoffsEl = ``;
    let el =``;
    let paid = 0
    modelPayoffs.forEach(item => {
        let price = parseInt(item.Price.replace(/,/g, ''));
        paid += price;
        payoffsEl += `
            <tr>
                <td>${item.TypeDisplay}</td>
                <td>${item.Description}</td>
                <td>${item.Date}</td>
                <td>${item.DueDate}</td>
                <td>${item.Price}</td>
                <td> <button type="button" class="btn btn-danger deletePayoff" data-id="${item.Id}"> حذف</button></td>
            </tr>
        `
    })
    modelFiles.forEach(item => {
        el += `
                <div class="col-3">
                    <div class="card">
                        <img src="${url + item.SavedName}" class="card-img-top"
                             alt=""/>
                        <div class="card-body">
                            <p class="card-text">${item.Description}</p>
                            <a class="btn btn-danger">حذف</a>
                        </div>

                    </div>
                </div>`
    })
    let invoiceAmount = parseInt(invoiceAmountStr);
    let remain = invoiceAmount - paid;
    
    modal.find('.images').html(el)
    modal.find('#payoffs-table').html(payoffsEl)
    modal.find('#description-el').text(description)
    modal.find('#cost-group-el').text(costGroup)
    modal.find('#payoffs-amount-el').text(paid.toLocaleString("en-US"))
    modal.find('#invoice-amount-el').text(invoiceAmount.toLocaleString("en-US"))
    modal.find('#remain-amount-el').text(remain.toLocaleString("en-US"))
    modal.find('#date-el').text(date)
    modal.find('#creditor-el').text(creditor)
    modal.find('.id').val(id)
    modal.find('[name=isDone]').prop("checked", false);
    modal.find('[name=discount]').val(discount);
})

$(".update-invoice").click(function(){
    const modal = $('#detailModal')
    let id = modal.find("[name=Id]").val()
    let discount = modal.find("[name=discount]").val()
    let isDone = modal.find("[name=isDone]").prop("checked")


    let data = {discount, isDone, id}
    $.ajax({
        url: `/Admin/api/Invoice/Update`,
        contentType: 'application/json',
        data: JSON.stringify(data),
        method: "post",
        success: () => {
            Swal.fire({
                type: "success",
                title: "ثبت شد",
                text: "اطلاعات با موفقیت ثبت شد"
            }).then(() => location.reload());
        }
    })
})