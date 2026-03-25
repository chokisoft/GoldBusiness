"use strict";
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || (function () {
    var ownKeys = function(o) {
        ownKeys = Object.getOwnPropertyNames || function (o) {
            var ar = [];
            for (var k in o) if (Object.prototype.hasOwnProperty.call(o, k)) ar[ar.length] = k;
            return ar;
        };
        return ownKeys(o);
    };
    return function (mod) {
        if (mod && mod.__esModule) return mod;
        var result = {};
        if (mod != null) for (var k = ownKeys(mod), i = 0; i < k.length; i++) if (k[i] !== "default") __createBinding(result, mod, k[i]);
        __setModuleDefault(result, mod);
        return result;
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
const fs = __importStar(require("fs"));
const path = __importStar(require("path"));
const glob = __importStar(require("glob"));
const root = process.cwd();
const clientTrans = path.join(root, 'GoldBusiness.Client', 'src', 'app', 'services', 'translation.service.ts');
if (!fs.existsSync(clientTrans)) {
    console.error('translation.service.ts not found at', clientTrans);
    process.exit(1);
}
const transText = fs.readFileSync(clientTrans, 'utf8');
// extract keys defined in translation.service.ts
const clientKeyRegex = /['"]([a-z0-9A-Z_.-]+)['"]\s*:\s*\{\s*['"]es['"]\s*:/g;
const clientKeys = new Set();
let m;
while ((m = clientKeyRegex.exec(transText)) !== null)
    clientKeys.add(m[1]);
// scan project files for translate usages
const patterns = [
    '**/*.html',
    '**/*.ts'
];
// gather used translation keys
const usedKeys = new Set();
const translateCallRegex = /translate\(\s*['"]([^'"]+)['"]/g;
const pipeTranslateRegex = /['"]([^'"]+)['"]\s*\|\s*translate/g;
const doubleCurlyPipeRegex = /{{\s*['"]([^'"]+)['"]\s*\|\s*translate/g;
for (const pat of patterns) {
    const files = glob.sync(pat, { cwd: root, ignore: ['**/node_modules/**', '**/dist/**', '**/.git/**'] });
    for (const f of files) {
        const full = path.join(root, f);
        let content = '';
        try {
            content = fs.readFileSync(full, 'utf8');
        }
        catch {
            continue;
        }
        let mm;
        while ((mm = translateCallRegex.exec(content)) !== null)
            usedKeys.add(mm[1]);
        while ((mm = pipeTranslateRegex.exec(content)) !== null)
            usedKeys.add(mm[1]);
        while ((mm = doubleCurlyPipeRegex.exec(content)) !== null)
            usedKeys.add(mm[1]);
    }
}
// Filter used keys that look like placeholders/help (heuristic)
function looksLikePlaceholderOrHelp(k) {
    const lower = k.toLowerCase();
    return lower.includes('placeholder') || lower.includes('help') || lower.endsWith('.placeholder') || lower.endsWith('help') || lower.includes('codigoplaceholder') || lower.includes('descripcionplaceholder');
}
const usedPlaceholderHelp = Array.from(usedKeys).filter(looksLikePlaceholderOrHelp).sort();
const clientPlaceholderHelp = Array.from(clientKeys).filter(looksLikePlaceholderOrHelp).sort();
// compute differences
const missingInClient = usedPlaceholderHelp.filter(k => !clientKeys.has(k));
const clientOnly = clientPlaceholderHelp.filter(k => !usedKeys.has(k));
const report = {
    generatedAt: new Date().toISOString(),
    counts: {
        totalClientTranslationKeys: clientKeys.size,
        totalUsedTranslationKeys: usedKeys.size,
        usedPlaceholderHelpCount: usedPlaceholderHelp.length,
        clientPlaceholderHelpCount: clientPlaceholderHelp.length,
        missingInClient: missingInClient.length,
        clientOnly: clientOnly.length
    },
    missingInClient,
    clientOnly,
    sampleUsedPlaceholderHelp: usedPlaceholderHelp.slice(0, 200),
    sampleClientPlaceholderHelp: clientPlaceholderHelp.slice(0, 200)
};
if (!fs.existsSync(path.join(root, 'tools')))
    fs.mkdirSync(path.join(root, 'tools'));
fs.writeFileSync(path.join(root, 'tools', 'placeholders-help-report.json'), JSON.stringify(report, null, 2), 'utf8');
console.log('Report saved to tools/placeholders-help-report.json');
console.log('missingInClient:', missingInClient.length, 'clientOnly:', clientOnly.length);
