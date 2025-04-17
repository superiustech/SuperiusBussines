const FormatadorValores = {
    converterParaInteiro: (valor) => {
        if (!valor) return 0;
        const apenasNumeros = valor.toString().replace(/\D/g, '');
        return parseInt(apenasNumeros) || 0;
    },

    converterParaDecimal: (valor) => {
        if (!valor) return 0.0;
        const valorDecimal = valor.toString().replace(/\./g, '').replace(/,/g, '.');
        return parseFloat(valorDecimal) || 0.0;
    }
};

export default FormatadorValores;