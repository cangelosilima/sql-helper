# Plano de Testes - SSMS Query Add-in

## 🎯 Objetivo
Validar o funcionamento completo do add-in em diferentes cenários e versões do SSMS.

## 🧪 Casos de Teste

### TC001 - Instalação Básica
**Objetivo:** Verificar instalação padrão
**Passos:**
1. Executar script de instalação
2. Verificar arquivos copiados
3. Verificar registro COM
4. Abrir SSMS
5. Verificar menu Tools

**Resultado Esperado:** Menu "Query Helper" visível em Tools

---

### TC002 - Funcionalidade Principal
**Objetivo:** Testar criação de nova query
**Passos:**
1. Clicar em Tools > Query Helper
2. Digitar query no TextBox
3. Clicar em "Nova Query"
4. Verificar nova janela criada

**Resultado Esperado:** Nova janela com query inserida

---

### TC003 - Validação de Entrada
**Objetivo:** Testar validação de dados
**Passos:**
1. Deixar TextBox vazio
2. Clicar em "Nova Query"
3. Verificar mensagem de aviso

**Resultado Esperado:** Alerta "Digite uma query antes de continuar!"

---

## 📊 Matriz de Compatibilidade

| Versão SSMS | Windows 10 | Windows 11 | .NET 4.7.2 | .NET 4.8 |
|-------------|------------|------------|-------------|----------|
| 2016 (13.0) | ✅ | ✅ | ✅ | ✅ |
| 2017 (14.0) | ✅ | ✅ | ✅ | ✅ |
| 2018 (15.0) | ✅ | ✅ | ✅ | ✅ |
| 2019 (15.0) | ✅ | ✅ | ✅ | ✅ |
| 2022 (19.0) | ✅ | ✅ | ✅ | ✅ |

---

## 📝 Checklist de Teste

### Pré-Release
- [ ] Compilação sem warnings
- [ ] Todos os TCs passaram
- [ ] Script de diagnóstico funciona
- [ ] Documentação atualizada

### Release
- [ ] Arquivo ZIP criado
- [ ] Scripts de instalação testados
- [ ] Compatibilidade verificada
- [ ] Performance aceitável
