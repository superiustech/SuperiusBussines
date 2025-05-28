import React, { useState, useCallback, useMemo, useEffect } from 'react';
import { AgGridReact } from 'ag-grid-react';
import { useNavigate } from 'react-router-dom';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import funcionalidades from '../common/Funcionalidades';
import { useAuth } from '../common/AuthContext';

const EstoqueProdutoTabela = ({ estoqueProdutos, loading, onDeletarProduto, onEntradaEstoque, onSaidaEstoque, onRefresh }) => {

    const [gridApi, setGridApi] = useState(null);
    const { validarFuncionalidade } = useAuth();
    const [selected, setSelected] = useState([]);

    const navigate = useNavigate();

    const columnDefs = useMemo(() => [
        { headerCheckboxSelection: true, checkboxSelection: true, headerName: "", field: "checkbox", width: 20, pinned: 'left', suppressMenu: true, suppressSorting: true, suppressFilter: true },
        { headerName: "#", field: "codigoProduto", sortable: true, filter: true, resizable: true, width: 80 },
        { headerName: "Qt. Estoque", field: "quantidadeEstoque", sortable: true, filter: true, resizable: true, width: 100 },
        { headerName: "Produto", field: "nomeProduto", sortable: true, filter: true, resizable: true, flex: 3, minWidth: 350 },
        { headerName: "Vl. Venda", field: "valorVenda", sortable: true, filter: true, resizable: true, flex: 1, minWidth: 70 }
    ], []);

    const gridOptions = useMemo(() => ({
        defaultColDef: { sortable: true, filter: true, resizable: true, minWidth: 100, flex: 1 },
        rowSelection: 'multiple',
        pagination: true,
        paginationPageSize: 10
    }), []);

    useEffect(() => {
        if (gridApi && estoqueProdutos.length) {
            gridApi.sizeColumnsToFit();
        }
    }, [gridApi, estoqueProdutos]);

    return (
        <div>
            <>
                <div className="mt-4 mb-3 d-flex justify-content-between align-items-center">
                    <div className="d-flex align-items-center">
                        {validarFuncionalidade(funcionalidades.DAR_ENTRADA_ESTOQUE) && (<button className="btn btn-primary me-2" onClick={() => onEntradaEstoque()}> Entrada de estoque </button>)}
                        {validarFuncionalidade(funcionalidades.DAR_SAIDA_ESTOQUE) && (<button className="btn btn-secondary me-2" onClick={() => onSaidaEstoque()}> Venda </button>)}
                        {validarFuncionalidade(funcionalidades.INATIVAR_PRODUTOS_ESTOQUE) && (<button className="btn btn-danger" disabled={selected.length === 0} onClick={() => onDeletarProduto(selected.map(item => item.codigoProduto).join(","))}> Excluir </button>)}
                    </div>
                    <div>
                        <button onClick={onRefresh} className="btn btn-outline-secondary" disabled={loading}> {loading ? 'Atualizando...' : 'Atualizar Lista'}</button>
                    </div>
                </div>

                <div className="ag-theme-alpine" style={{ height: 250, width: '100%' }}>
                    <AgGridReact
                        key={estoqueProdutos.length}
                        rowData={estoqueProdutos || []}
                        columnDefs={columnDefs}
                        onGridReady={(params) => setGridApi(params.api)}
                        gridOptions={gridOptions}
                        suppressReactUi={true}
                        animateRows={true}
                        rowHeight={48}
                        onSelectionChanged={(event) => {
                            const selectedNodes = event.api.getSelectedNodes();
                            const selectedData = selectedNodes.map(node => node.data);
                            setSelected(selectedData);
                        }}
                        headerHeight={56} />
                </div>
            </>
        </div>
    );
};

export default EstoqueProdutoTabela;
