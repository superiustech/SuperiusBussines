import React, { useEffect, useRef } from 'react';
import $ from 'jquery';
import 'jquery-mask-plugin';

const maskTypes = {
    date: '00/00/0000',
    time: '00:00:00',
    date_time: '00/00/0000 00:00:00',
    cep: '00000-000',
    phone: '0000-0000',
    phone_with_ddd: '(00) 0000-0000',
    phone_us: '(000) 000-0000',
    mixed: 'AAA 000-S0S',
    cpf: '000.000.000-00',
    cnpj: '00.000.000/0000-00',
    money: '000.000.000.000.000,00',
    money2: '#.##0,00',
    percent: '##0,00%',
    numero_inteiro: '#00000000'
};

const InputMasked = ({ type, placeholder, className, value, onChange, ...props }) => {
    const inputRef = useRef(null);

    useEffect(() => {
        if (inputRef.current && maskTypes[type]) {
            const options = {};

            if (type === 'cpf' || type === 'cnpj') {
                options.reverse = true;
            } else if (type === 'money2') {
                options.reverse = true;
            } else if (type === 'ip_address') {
                options.translation = {
                    'Z': { pattern: /[0-9]/, optional: true }
                };
            } else if (type === 'numero_inteiro') {
                options.translation = {
                    '#': { pattern: /[1-9]/ },
                    '0': { pattern: /[0-9]/ }
                };
            }

            $(inputRef.current).mask(maskTypes[type], options);
        }

        return () => {
            if (inputRef.current) {
                $(inputRef.current).unmask();
            }
        };
    }, [type]);

    return (
        <input
            ref={inputRef}
            type="text"
            className={`form-control ${className || ''}`}
            placeholder={placeholder}
            value={value}
            onChange={onChange}
            {...props}
        />
    );
};

export default InputMasked;