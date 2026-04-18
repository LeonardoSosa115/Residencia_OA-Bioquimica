using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AtomInfoPanel : MonoBehaviour
{
    [Header("Info física")]
    public TextMeshProUGUI txtNumeroAtomico;
    public TextMeshProUGUI txtNumeroDeMasa;
    public TextMeshProUGUI txtCargaNeta;
    public TextMeshProUGUI txtEstabilidad;

    [Header("Colores de estabilidad")]
    public Color colorEstable   = new Color(0.2f, 0.8f, 0.2f);
    public Color colorInestable = new Color(0.9f, 0.2f, 0.2f);

    [Header("Tabla periódica")]
    public Transform periodicTableContainer;
    public GameObject cellPrefab;

    // Referencia al átomo
    private AtomController atomController;

    // Celda actualmente resaltada
    private GameObject highlightedCell;

    // Datos de la tabla periódica simplificada
    // formato: { simbolo, numero atomico, fila, columna }
    private readonly (string symbol, int z, int row, int col)[] elements =
    {
        // Periodo 1
        ("H",   1,  1,  1), ("He",  2,  1, 18),
        // Periodo 2
        ("Li",  3,  2,  1), ("Be",  4,  2,  2),
        ("B",   5,  2, 13), ("C",   6,  2, 14), ("N",   7,  2, 15),
        ("O",   8,  2, 16), ("F",   9,  2, 17), ("Ne", 10,  2, 18),
        // Periodo 3
        ("Na", 11,  3,  1), ("Mg", 12,  3,  2),
        ("Al", 13,  3, 13), ("Si", 14,  3, 14), ("P",  15,  3, 15),
        ("S",  16,  3, 16), ("Cl", 17,  3, 17), ("Ar", 18,  3, 18),
        // Periodo 4
        ("K",  19,  4,  1), ("Ca", 20,  4,  2),
        ("Sc", 21,  4,  3), ("Ti", 22,  4,  4), ("V",  23,  4,  5),
        ("Cr", 24,  4,  6), ("Mn", 25,  4,  7), ("Fe", 26,  4,  8),
        ("Co", 27,  4,  9), ("Ni", 28,  4, 10), ("Cu", 29,  4, 11),
        ("Zn", 30,  4, 12), ("Ga", 31,  4, 13), ("Ge", 32,  4, 14),
        ("As", 33,  4, 15), ("Se", 34,  4, 16), ("Br", 35,  4, 17),
        ("Kr", 36,  4, 18),
        // Periodo 5
        ("Rb", 37,  5,  1), ("Sr", 38,  5,  2),
        ("Y",  39,  5,  3), ("Zr", 40,  5,  4), ("Nb", 41,  5,  5),
        ("Mo", 42,  5,  6), ("Tc", 43,  5,  7), ("Ru", 44,  5,  8),
        ("Rh", 45,  5,  9), ("Pd", 46,  5, 10), ("Ag", 47,  5, 11),
        ("Cd", 48,  5, 12), ("In", 49,  5, 13), ("Sn", 50,  5, 14),
        ("Sb", 51,  5, 15), ("Te", 52,  5, 16), ("I",  53,  5, 17),
        ("Xe", 54,  5, 18),
        // Periodo 6
        ("Cs", 55,  6,  1), ("Ba", 56,  6,  2),
        ("La", 57,  6,  3),
        ("Hf", 72,  6,  4), ("Ta", 73,  6,  5), ("W",  74,  6,  6),
        ("Re", 75,  6,  7), ("Os", 76,  6,  8), ("Ir", 77,  6,  9),
        ("Pt", 78,  6, 10), ("Au", 79,  6, 11), ("Hg", 80,  6, 12),
        ("Tl", 81,  6, 13), ("Pb", 82,  6, 14), ("Bi", 83,  6, 15),
        ("Po", 84,  6, 16), ("At", 85,  6, 17), ("Rn", 86,  6, 18),
        // Periodo 7
        ("Fr", 87,  7,  1), ("Ra", 88,  7,  2),
        ("Ac", 89,  7,  3),
        ("Rf",104,  7,  4), ("Db",105,  7,  5), ("Sg",106,  7,  6),
        ("Bh",107,  7,  7), ("Hs",108,  7,  8), ("Mt",109,  7,  9),
        ("Ds",110,  7, 10), ("Rg",111,  7, 11), ("Cn",112,  7, 12),
        ("Nh",113,  7, 13), ("Fl",114,  7, 14), ("Mc",115,  7, 15),
        ("Lv",116,  7, 16), ("Ts",117,  7, 17), ("Og",118,  7, 18),
        // Lantánidos (fila 9)
        ("Ce", 58,  9,  4), ("Pr", 59,  9,  5), ("Nd", 60,  9,  6),
        ("Pm", 61,  9,  7), ("Sm", 62,  9,  8), ("Eu", 63,  9,  9),
        ("Gd", 64,  9, 10), ("Tb", 65,  9, 11), ("Dy", 66,  9, 12),
        ("Ho", 67,  9, 13), ("Er", 68,  9, 14), ("Tm", 69,  9, 15),
        ("Yb", 70,  9, 16), ("Lu", 71,  9, 17),
        // Actínidos (fila 10)
        ("Th", 90, 10,  4), ("Pa", 91, 10,  5), ("U",  92, 10,  6),
        ("Np", 93, 10,  7), ("Pu", 94, 10,  8), ("Am", 95, 10,  9),
        ("Cm", 96, 10, 10), ("Bk", 97, 10, 11), ("Cf", 98, 10, 12),
        ("Es", 99, 10, 13), ("Fm",100, 10, 14), ("Md",101, 10, 15),
        ("No",102, 10, 16), ("Lr",103, 10, 17),
    };

    // Neutrones estables aproximados por elemento
    private readonly Dictionary<int, (int min, int max)> stableNeutrons =
        new Dictionary<int, (int, int)>()
    {
        {1,  (0,  1)},  {2,  (1,  2)},  {3,  (3,  4)},  {4,  (5,  5)},
        {5,  (5,  6)},  {6,  (6,  7)},  {7,  (7,  8)},  {8,  (8,  10)},
        {9,  (10, 10)}, {10, (10, 12)}, {11, (12, 12)}, {12, (12, 14)},
        {13, (14, 14)}, {14, (14, 16)}, {15, (16, 16)}, {16, (16, 18)},
        {17, (18, 20)}, {18, (18, 22)}, {19, (20, 22)}, {20, (20, 24)},
        {21, (24, 24)}, {22, (24, 26)}, {23, (26, 28)}, {24, (26, 30)},
        {25, (28, 30)}, {26, (28, 32)}, {27, (30, 32)}, {28, (30, 36)},
        {29, (34, 36)}, {30, (34, 38)}, {31, (38, 40)}, {32, (38, 44)},
        {33, (42, 44)}, {34, (40, 48)}, {35, (44, 46)}, {36, (44, 50)},
        {37, (48, 50)}, {38, (48, 52)}, {39, (50, 52)}, {40, (50, 56)},
        {41, (52, 52)}, {42, (52, 58)}, {44, (54, 64)}, {45, (58, 60)},
        {46, (56, 66)}, {47, (60, 62)}, {48, (60, 68)}, {49, (64, 68)},
        {50, (62, 74)}, {51, (70, 74)}, {52, (66, 78)}, {53, (74, 78)},
        {54, (70, 82)}, {55, (78, 82)}, {56, (74, 82)},
    };

    void Start()
    {
        atomController = FindObjectOfType<AtomController>();
        GenerarTabla();
    }

    // ── Tabla periódica ───────────────────────────────────────

    void GenerarTabla()
    {
        if (periodicTableContainer == null || cellPrefab == null) return;

        // Tamaño de celda
        float cellSize = 22f;
        float spacing  = 1f;

        foreach (var el in elements)
        {
            GameObject cell = Instantiate(cellPrefab, periodicTableContainer);
            RectTransform rt = cell.GetComponent<RectTransform>();

            rt.sizeDelta = new Vector2(cellSize, cellSize);
            rt.anchoredPosition = new Vector2(
                (el.col - 1) * (cellSize + spacing),
                -(el.row - 1) * (cellSize + spacing)
            );

            TextMeshProUGUI txt = cell.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null) txt.text = el.symbol;

            // Guarda el número atómico en el nombre del objeto
            cell.name = "Cell_" + el.z;

            Image img = cell.GetComponent<Image>();
            if (img != null) img.color = new Color(0.15f, 0.25f, 0.35f);
        }
    }

    void ResaltarElemento(int z)
    {
        // Quita resaltado anterior
        if (highlightedCell != null)
        {
            Image img = highlightedCell.GetComponent<Image>();
            if (img != null) img.color = new Color(0.15f, 0.25f, 0.35f);
        }

        if (z <= 0) return;

        // Busca la celda del elemento actual
        Transform cell = periodicTableContainer.Find("Cell_" + z);
        if (cell == null) return;

        Image cellImg = cell.GetComponent<Image>();
        if (cellImg != null) cellImg.color = new Color(0.2f, 0.5f, 0.9f);

        highlightedCell = cell.gameObject;
    }

    // ── Info física ───────────────────────────────────────────

    public void ActualizarInfo()
    {
        if (atomController == null) return;

        int z  = atomController.ProtonCount;
        int n  = atomController.NeutronCount;
        int e  = atomController.ElectronCount;
        int a  = z + n;
        int carga = z - e;

        if (txtNumeroAtomico != null)
            txtNumeroAtomico.text = z.ToString();

        if (txtNumeroDeMasa != null)
            txtNumeroDeMasa.text = a.ToString();

        if (txtCargaNeta != null)
            txtCargaNeta.text = carga == 0 ? "0" : (carga > 0 ? "+" + carga : carga.ToString());

        if (txtEstabilidad != null)
        {
            bool estable = EsEstable(z, n);
            txtEstabilidad.text  = estable ? "Estable" : "Inestable";
            txtEstabilidad.color = estable ? colorEstable : colorInestable;
        }

        ResaltarElemento(z);
    }

    bool EsEstable(int z, int n)
    {
        if (z == 0) return true;

        if (stableNeutrons.TryGetValue(z, out var range))
            return n >= range.min && n <= range.max;

        // Para elementos no definidos: aproximación general
        float ratio = z > 0 ? (float)n / z : 0;
        return ratio >= 1.0f && ratio <= 1.5f;
    }
}