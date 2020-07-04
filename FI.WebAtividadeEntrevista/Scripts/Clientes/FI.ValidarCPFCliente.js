
$('#formCadastro #CPF').blur(function () {
    $("#btn-salvar").attr("disabled", false);
    $('#mensagem-erro-cpf').text('').hide();
    let nCPF = $("#CPF").val()

    let retornoCPF = validarCPF(nCPF);
    if (retornoCPF) {
        VerificarCPFDublicado(nCPF);
    } else {
        $("#btn-salvar").attr("disabled", true);
        $('#mensagem-erro-cpf').text("CPF invalido").show();
    }

});



function VerificarCPFDublicado(cpf) {
    debugger;

    $.ajax({
        url: '../../Cliente/VerificarCPFDublicado',
        method: "POST",
        data: {
            "CPF": cpf
        },
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                if (r.Message.length) {
                    $("#btn-salvar").attr("disabled", true);
                    $("#mensagem-erro-cpf").text(r.Message).show();
                }

            }
    });

}


function validarCPF(cpf) {
    cpf = cpf.replace(/[^\d]+/g, '');
    if (cpf == '') return false;
    // Elimina CPFs invalidos conhecidos	
    if (cpf.length != 11 ||
        cpf == "00000000000" ||
        cpf == "11111111111" ||
        cpf == "22222222222" ||
        cpf == "33333333333" ||
        cpf == "44444444444" ||
        cpf == "55555555555" ||
        cpf == "66666666666" ||
        cpf == "77777777777" ||
        cpf == "88888888888" ||
        cpf == "99999999999")
        return false;
    // Valida 1o digito	
    add = 0;
    for (i = 0; i < 9; i++)
        add += parseInt(cpf.charAt(i)) * (10 - i);
    rev = 11 - (add % 11);
    if (rev == 10 || rev == 11)
        rev = 0;
    if (rev != parseInt(cpf.charAt(9)))
        return false;
    // Valida 2o digito	
    add = 0;
    for (i = 0; i < 10; i++)
        add += parseInt(cpf.charAt(i)) * (11 - i);
    rev = 11 - (add % 11);
    if (rev == 10 || rev == 11)
        rev = 0;
    if (rev != parseInt(cpf.charAt(10)))
        return false;
    return true;
}
