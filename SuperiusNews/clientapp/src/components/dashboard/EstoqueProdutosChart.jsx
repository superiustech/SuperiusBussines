import React from 'react';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

const EstoqueProdutosChart = ({ produtosEstoques }) => {
    return (
        <div style={{ width: '100%', height: '400px' }}>
            <h6 className="fw-bold fs-3 text-primary my-4 px-5 text-end">
                <i className="bi bi-speedometer2 me-2"></i> Produtos por estoques
            </h6>
            <ResponsiveContainer width="100%" height="100%">
                <BarChart data={produtosEstoques}>
                    <XAxis dataKey="nomeProduto" />
                    <YAxis />
                    <Tooltip />
                    <Legend />
                    {produtosEstoques.length > 0 &&
                        Object.keys(produtosEstoques[0])
                            .filter(key => key !== "nomeProduto")
                            .map((estoqueNome, index) => (
                                <Bar
                                    key={estoqueNome}
                                    dataKey={estoqueNome}
                                    stackId="estoque" 
                                    fill={["#0052cc", "#0041a8", "#00338a", "#00256d", "#001751","#000c36", "#004b99", "#003f7a", "#005288","#002f5f"][index % 10]}
                                    name={`Estoque: ${estoqueNome}`}
                                />
                            ))
                    }
                </BarChart>

            </ResponsiveContainer>
        </div>
    );
};

export default EstoqueProdutosChart;