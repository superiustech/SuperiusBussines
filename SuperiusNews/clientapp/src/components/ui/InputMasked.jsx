import React from 'react';
import InputMask from 'react-input-mask';

const masks = {
    date: '00/00/0000',
    time: '00:00:00',
    date_time: '00/00/0000 00:00:00',
    cep: '00000-000',
    phone: '0000-0000',
    phone_with_ddd: '(00) 00000-0000',
    phone_us: '(000) 000-0000',
    mixed: 'AAA 000-S0S',
    cpf: '000.000.000-00',
    cnpj: '00.000.000/0000-00',
    money: '000.000.000.000.000,00',
    money2: '#.##0,00',
    percent: '##0,00%',
    numero_inteiro: '#00000000',
};

const MaskedInput = ({ type, className = '', ...props }) => {
    const mask = masks[type] || null;

    return mask ? (
        <InputMask
            mask={mask}
            {...props}
            className={`form-control ${className}`}
        />
    ) : (
        <input
            ref={inputRef}
            type="text"
            className={`form-control ${className || ''}`}
            placeholder={placeholder}
            value={value}
            onChange={onChange}
            {...props}
            className={`form-control ${className}`}
        />
    );
};

export default MaskedInput; // Exportação padrão