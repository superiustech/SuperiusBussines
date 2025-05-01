import React, { useState, useCallback, useMemo, useEffect } from 'react';
import { AgGridReact } from 'ag-grid-react';
import { useNavigate } from 'react-router-dom';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';

const EstoqueProdutoHistoricoTable = ({ historico, loading, onEntradaEstoque, onSaidaEstoque, onRefresh }) => {
    const [gridApi, setGridApi] = useState(null);
    const [selected, setSelected] = useState([]);
    const navigate = useNavigate();

    const columnDefs = useMemo(() => [
        { headerName: "#", field: "codigo", sortable: true, filter: true, resizable: true, width: 80 },
        { headerName: "Tipo movimentação", field: "tipoMovimentacao", sortable: true, filter: true, resizable: true, width: 100 },
        { headerName: "Dt. Movimentação", field: "dataMovimentacao", sortable: true, filter: 'agDateColumnFilter', resizable: true, width: 120,
            valueFormatter: params => {
                if (!params.value) return '';
                const date = new Date(params.value);
                return date.toLocaleDateString('pt-BR'); 
            }
        },
        { headerName: "Produto", field: "produto", sortable: true, filter: true, resizable: true, flex: 3, minWidth: 350 },
        { headerName: "Qt.", field: "quantidadeMovimentada", sortable: true, filter: true, resizable: true, flex: 1, minWidth: 70 },
        { headerName: "E. Origem", field: "estoqueOrigem", sortable: true, filter: true, resizable: true, flex: 1, minWidth: 150 },
        { headerName: "E. Destino", field: "estoqueDestino", sortable: true, filter: true, resizable: true, flex: 1, minWidth: 150 },
        { headerName: "Observação", field: "observacao", sortable: true, filter: true, resizable: true, flex: 1, minWidth: 150 }
    ], []);

    const gridOptions = useMemo(() => ({
        defaultColDef: { sortable: true, filter: true, resizable: true, minWidth: 100, flex: 1 },
        rowSelection: 'multiple',
        pagination: true,
        paginationPageSize: 10
    }), []);

    useEffect(() => {
        if (gridApi && historico.length) {
            gridApi.sizeColumnsToFit();
        }
    }, [gridApi, historico]);

    return (
        <div>
            <>
            <div className="mt-4 mb-3 d-flex justify-content-between align-items-center">
                <div className="d-flex align-items-center">
                    <button className="btn btn-primary me-2" onClick={() => onEntradaEstoque()}> Entrada de estoque </button>
                    <button className="btn btn-secondary me-2" onClick={() => onSaidaEstoque()}> Venda </button>
                </div>
                <div>
                    <button onClick={onRefresh} className="btn btn-outline-secondary" disabled={loading}> {loading ? 'Atualizando...' : 'Atualizar Lista'}</button>
                </div>
            </div>

            <div className="ag-theme-alpine" style={{ height: 450, width: '100%' }}>
                <AgGridReact
                    key={historico.length}
                    rowData={historico || []}
                    columnDefs={columnDefs}
                    onGridReady={(params) => setGridApi(params.api)}
                    gridOptions={gridOptions}
                    suppressReactUi={true}
                    animateRows={true}
                    rowHeight={48}
                    headerHeight={56}/>
            </div>
            </>
        </div>
    );
};

export default EstoqueProdutoHistoricoTable;
