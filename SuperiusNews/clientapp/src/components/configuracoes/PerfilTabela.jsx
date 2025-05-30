import React, { useState, useMemo, useEffect } from 'react';
import { AgGridReact } from 'ag-grid-react';
import { useNavigate } from 'react-router-dom';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import funcionalidadesArray from '../common/Funcionalidades';
import { useAuth } from '../common/AuthContext';
import '../../styles/ag-custom.css';

const PerfilTabela = ({ perfis, loading, onEditarPerfil, onAtivarPerfis, onInativarPerfis, onPerfis, onRefresh }) => {
    const [gridApi, setGridApi] = useState(null);
    const [selected, setSelected] = useState([]);
    const { validarFuncionalidade } = useAuth();

    const navigate = useNavigate();

    const columnDefs = useMemo(() => [
        { headerCheckboxSelection: true, checkboxSelection: true, headerName: "", field: "checkbox",width: 20, pinned: 'left', suppressMenu: true,suppressSorting: true, suppressFilter: true},
        { headerName: "#", field: "codigoPerfil", sortable: true, filter: true, resizable: true, width: 80 },
        { headerName: "Nome", field: "nomePerfil", sortable: true, filter: true, resizable: true, width: 100 },
        { headerName: "Descrição", field: "descricaoPerfil", sortable: true, filter: true, resizable: true, flex: 3, minWidth: 350 },
        { headerName: "Status", field: "ativa", sortable: true, filter: true, resizable: true, flex: 3, minWidth: 250 }
    ], []);

    const gridOptions = useMemo(() => ({
        defaultColDef: { sortable: true, filter: true, resizable: true, minWidth: 100, flex: 1 },
        rowSelection: 'multiple',
        pagination: true,
        paginationPageSize: 10
    }), []);

    useEffect(() => {
        if (gridApi && perfis.length) {
            gridApi.sizeColumnsToFit();
        }
    }, [gridApi, perfis]);

    return (
        <div>
            <>
            <div className="mt-4 mb-3 d-flex justify-content-between align-items-center">
                <div className="d-flex align-items-center">
                    {validarFuncionalidade(funcionalidadesArray.VISUALIZAR_PERFIS) && (<button className="btn btn-primary me-2" disabled={selected.length !== 1} onClick={() => onEditarPerfil(selected[0].codigoPerfil)}> Editar </button>)}
                    {validarFuncionalidade(funcionalidadesArray.ASSOCIAR_PERMISSAO_PERFIL) && (<button className="btn btn-secondary me-2 btn-warning" disabled={selected.length !== 1} onClick={() => onPerfis(selected[0].codigoPerfil)}> Permissões </button>)}
                    {validarFuncionalidade(funcionalidadesArray.ATIVAR_PERFIS) && (<button className="btn btn-dark me-2" disabled={selected.length === 0} onClick={() => onAtivarPerfis(selected.map(item => item.codigoPerfil).join(","))}> Ativar </button>)}
                    {validarFuncionalidade(funcionalidadesArray.INATIVAR_PERFIS) && (<button className="btn btn-secondary me-2 btn-danger" disabled={selected.length === 0} onClick={() => onInativarPerfis(selected.map(item => item.codigoPerfil).join(","))}> Inativar </button>)}
                </div>
                <div>
                    <button onClick={onRefresh} className="btn btn-outline-secondary" disabled={loading}> {loading ? 'Atualizando...' : 'Atualizar Lista'}</button>
                </div>
            </div>

            <div className="ag-theme-alpine custom" style={{ height: 450, width: '100%' }}>
                <AgGridReact
                    key={perfis.length}
                    rowData={perfis || []}
                    columnDefs={columnDefs}
                    onGridReady={(params) => setGridApi(params.api)}
                    gridOptions={gridOptions}
                    suppressReactUi={true}
                    animateRows={true}
                    onSelectionChanged={(event) => {
                        const selectedNodes = event.api.getSelectedNodes();
                        const selectedData = selectedNodes.map(node => node.data);
                        setSelected(selectedData);
                    }}
                    rowHeight={48}
                    headerHeight={56}/>
            </div>
            </>
        </div>
    );
};

export default PerfilTabela;
