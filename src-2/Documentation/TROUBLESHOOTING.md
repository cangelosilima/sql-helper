# Guia de Solução de Problemas

## 🚨 Problemas Comuns

### ❌ Add-in não aparece no menu Tools

**Sintomas:**
- Menu "Query Helper" não está visível em Tools
- Add-in Manager não mostra o add-in

**Soluções:**

1. **Verificar localização dos arquivos:**
   ```cmd
   dir "%USERPROFILE%\Documents\Visual Studio 2017\Addins\*SSMS*"
   ```

2. **Reregistrar assembly COM:**
   ```cmd
   regasm "SSMSQueryAddin.dll" /unregister
   regasm "SSMSQueryAddin.dll" /codebase
   ```

3. **Verificar permissões:**
   ```cmd
   icacls "%USERPROFILE%\Documents\Visual Studio 2017\Addins\" /grant Users:F /T
   ```

---

### ❌ Erro "Assembly não encontrado"

**Soluções:**

1. **Verificar versão do .NET:**
   ```powershell
   Get-ItemProperty "HKLM:SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\" -Name Release
   ```

2. **Copiar dependências necessárias para a pasta de add-ins**

---

### ❌ Erro de permissão

**Soluções:**

1. **Executar como Administrador:**
   ```cmd
   regasm "SSMSQueryAddin.dll" /codebase
   ```

2. **Verificar UAC e configurações de segurança**

---

## 🔧 Ferramentas de Diagnóstico

### Script de Diagnóstico Automático
```powershell
.\Scripts\DiagnosticCheck.ps1
```

### Verificação Manual
1. Verificar arquivos essenciais
2. Verificar registro COM
3. Verificar processo SSMS
4. Verificar logs

---

## 📞 Suporte

Para problemas persistentes:
- Consulte a documentação completa
- Execute o script de diagnóstico
- Reporte issues no GitHub
