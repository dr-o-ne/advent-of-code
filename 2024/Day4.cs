﻿namespace AdventOfCode._2024;

public sealed class Day4
{
    internal record Pair(int y, int x);

    public int Solve1(string input)
    {
        var matrix = Utils.ParseMatrix<char>(input);

        var result = 0;

        for (int y = 0; y < matrix.GetLength(0); y++)
        {
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                Check((y, x), (y, x + 1), (y, x + 2), (y, x + 3));
                Check((y, x), (y, x - 1), (y, x - 2), (y, x - 3));
                Check((y, x), (y + 1, x), (y + 2, x), (y + 3, x));
                Check((y, x), (y - 1, x), (y - 2, x), (y - 3, x));
                Check((y, x), (y + 1, x + 1), (y + 2, x + 2), (y + 3, x + 3));
                Check((y, x), (y - 1, x - 1), (y - 2, x - 2), (y - 3, x - 3));
                Check((y, x), (y + 1, x - 1), (y + 2, x - 2), (y + 3, x - 3));
                Check((y, x), (y - 1, x + 1), (y - 2, x + 2), (y - 3, x + 3));
            }
        }

        return result;

        void Check((int y, int x) X, (int y, int x) M, (int y, int x) A, (int y, int x) S)
        {
            try
            {
                bool isValid = matrix[X.y, X.x] == 'X'
                            && matrix[M.y, M.x] == 'M'
                            && matrix[A.y, A.x] == 'A'
                            && matrix[S.y, S.x] == 'S';

                if(isValid)
                    result++;
            }
            catch (IndexOutOfRangeException)
            {
            }
        }
    }

    public int Solve2(string input)
    {
        var matrix = Utils.ParseMatrix<char>(input);

        var result = 0;

        for (int y = 0; y < matrix.GetLength(0); y++)
        {
            for (int x = 0; x < matrix.GetLength(1); x++)
            {
                /*M
                    A
                      S*/
                Check((y, x), (y - 1, x - 1), (y + 1, x + 1), (y + 1, x - 1), (y - 1, x + 1));
                Check((y, x), (y - 1, x - 1), (y + 1, x + 1), (y - 1, x + 1), (y + 1, x - 1));
                /*S
                    A
                      M*/
                Check((y, x), (y + 1, x + 1), (y - 1, x - 1), (y + 1, x - 1), (y - 1, x + 1));
                Check((y, x), (y + 1, x + 1), (y - 1, x - 1), (y - 1, x + 1), (y + 1, x - 1));
            }
        }

        return result;

        void Check((int y, int x) A, (int y, int x) M1, (int y, int x) S1, (int y, int x) M2, (int y, int x) S2)
        {
            try
            {
                bool isValid = matrix[A.y, A.x] == 'A' 
                            && matrix[M1.y, M1.x] == 'M'
                            && matrix[M2.y, M2.x] == 'M'
                            && matrix[S1.y, S1.x] == 'S'
                            && matrix[S2.y, S2.x] == 'S';

                if (isValid)
                    result++;
            }
            catch (IndexOutOfRangeException)
            {
            }
        }
    }

    [Fact]
    public void Test1() => Assert.Equal(2685, Solve1(Input));

    [Fact]
    public void Test2() => Assert.Equal(2048, Solve2(Input));

    private const string Input = @"ASAMXSMXMAXSAMMAMXAXXAXXMSAMXASAMXSASMSMXAXMAMMSSSMAMSMMSASMMSSMSMSMSXMMSXMXMAXXMAASMMMXMXMMXMAMXSSSMSMSMSMMMMAMMSMSXXAXMMXSAXMMXXAMASMXMASM
MMXXAMXAXSMXMMSSMMSSMSMMXSSMXMAASASAAAAMSMSMSSXAMXXAMXAAAAMXASXAAAAAMASASASMSSMMMSXSAMXAMAMSSMASAMAAAAAAXXAAXMAMXSAMAMSSMAMXMMSMMXSXAMAAASAS
SAMMAXMSMXMAXMAMAAMXAAASAMXSXASAMAMMMXMMAXMAAXMXMMXSSSMMSXXMXMASMSMSMXMASAMAAXAAAMMSMMXMXAXAASAMAMMMSMSMSSSMSSXSAMSMMMAAMXXXAAAAAMAMSSSXMXAX
AAMSAMMAMASXSMSMMMSMSMSMASAMMMMSMXMASXSSMSMMMSMSXAMXAASAMMMXSAMAAXAXMXMAMXMXXSXMMMAXASAAMSSSMMXSAMXAAXMAMXMAXAXMXAAXMMSSMMSMMSSSMAMMAAAMMMSM
MMMSAXSMXMXAMAXXAXXAAXXMXMASMAAXXAXMSAAXSAXAAAASMSMMAMMXSAXAXAXMMMMMMXMASXMSMMXSAMMSAMMXMXAMXMAAXXMSMSMSMMMSMMXSMAMMSAAAXAASXXAMASXMMSMAAAMA
XAAMAMXXSMMMMAMSSMMSMSXMXSXMMMXXMMSXMMMMSXSMSMXMAMMXAMMMMMMXMMMMMAXMAAMXSXAAAXAMAXMAMMMSSMSMAMASAMXAAAAAMXAAAXAXAASAXMSXMSSSMMMMAMAMXXXSMSSM
SMSSXSXAAMMAMXXAMAMXXXAMMSAMXMXSMMAXMASXMAMAAAAAXXXSMSXASXMSXSAMSMSMMMSAXMASMMXSXMASXAXAXAMSXSAXMSSMSMSMSMSSSMMMSXMASMAXAXMXMAAMSSSSMSAMAAMX
XAMXAMMSMMSASMMASMMXMSSMAXAMASAAAXMMSASAMAMSMSMSAMXSAAMSMAAAAXXXAXAAMXMASXXMMAMXMSXMSXSMMMMXXMMSAAXXAXXASAAAXMSMMASXMXAAMMXXMMXMAAMAAMAMMMSS
XSSMAMAXAASXXXSAMXMAXAAMSSSMAMXSMMSAMXSXSXXAAXAXMXAMMMSMSMMMSSSSMSSSMMMAMMAMSAMAMXAXSXSXMSAXXMAMMMMMMXMASXSMMXSAMAMSAMXSSSXMSSSMMSMMSMXMAAXA
AXAMSMSMMMSAXMMMMAMMMMSAMXMMXSAXXAMXSAXMXXSMMMMMXMSSMXAAAXXXMAAAAAMAMAMAMMAMMAMXSMMMMAXMASASXXAXXMXSSXMAMMMAMASMMSSMXSXMAMMXAAXMAXMXMXSXMMMS
SSSMXAXXXASMSMAXXAAAAAXAXAMXAMASMSMMXMSMXAMXMAMAAAMAMSMSMSSMMSSMMMMAMSSMSSSSSSMSMXSAMSMXMSAMXSMSSXAAAXMASXSAMASXMMAXASAMMMAMMSMMSMSXMAMXMAAA
MAAXMAMSMMSAAMAMXMMSMXSSMASAMMMMMMASMXAAMSMMMASMSMSAMXXAAMMXAXAAXXMAXXAAMAAXMASXAMSAMAAXXMMMXAAAXMSSSMXSSMSAMMSAMXAMXMAMXXMAAAAMAASXMASASMSM
MSMMMSMSAAMMMSMSASMMXXAXSASAMXMXMSAMAMMSMMASXXSXAAMAMXMMSMAMXSSMMSSXSSMMMMSMSXMMXXSAMXSMMSASXSMMSMXAAAXAXASMMASMMMSMXSSMSSSSSSMMMMMASMSASAXX
XMAMSAAXMMMAMAMXMXAAMMMMMMSAMXXAXMAXMSAAASAMXMMMMMMSXSAXAMASXAASAAXMAXXXXAMXSMXMMMMAMSXMASMSAAAXAMMMMSMMMXMAMAXAXXXAAAXAAAAMAMAXSSSMMXMAMXMA
XSAMXMSMXMSMSASXSMMMMAAAXMSAMXSSSMSMXMASXMAXSAAAMAAXAMXSXSAMMMMMMXSASMSMMMSASMAXAAMAMAXMXSAMXMMSMSMSAXAXSXMXMSSSMMAMMXMMMSMMAMXMAMXAXXMSMSMS
MMMMXSAXAAAASAMXSAMMXSMSMMSAMMXAMAAXXXXMASXSXMASMSSMXMAMXMAMAAAAXAXXAAAXAXMXMSAMXSSSSXMXMMXMASXAXMXMMSMMMXMAMXAAAAXXMXSXMXXMASXMXMSMMXSAAAAA
XAAXXMAMMSMXMSXASAMMXMAAAXSAMXMMMSAMXXXMXXXSXSAMXMAMSMSSMSAXSMSSMMSSMSMSSXMAXMMSXAAAAMAMSAMXAMXMAXAMXAMAXSSMSMSMMMSMMMAXMAMSASXSAMAXAXSMXMSM
SSSSSMSMXAXSAMMMSAMXAMMXSMSXMASXMXMSSMMSASMSXMASMSXMXAMAMMMMMAMAXMAXXAASMAMMSAASMMMMMAXAMASMSSSXSXMAMXSMSAAXSAXXXXXMASMSAAXMASASASMSSMMSAAAX
XAMAAAXASAXMAXXXSXMMMSAAXXMASASMMMMAAMXAASASAMXMASMMMMMXMASAMMSAMMAMSMSMMAMMMMMXAXXAXMSXSAMXAAAAAMXSXMMXAMXXSAMSMMXSXSXMAMSMAMXMASAAMAASXSSM
MAMSMMMAASXMMMMXMMXSSMMXSASXMAMMXXMSSMXMSMMMMXSAMXAMAMSMSASXSXMAMMMMAMAMSSSMMSASXMXMSXAAMMSMMMMMMSAXSXSXXXMMMMMXAAXMMMMMAXMMMSXXXMMMMMMSXMAM
SXMXSMMSMAXMAXSAMXAXAASMSAMXMMMSMSMMXMMMAMXMAMMAXMMMASAAMMSXMASXMXMXASMXAAAAAXMMSAASMXMXMASXSAASXMASMMSAMXXAAXXMXMMAAAASXMXAXMASMXAXXSAMASAM
AASAXMAMXAASMASASMMSSMMXMMMXXMAXAMMAXMAXAMAXMASAMXMSXSMSMAXASAMXMASXMMMMMSMMXMXAMSMXAAXAMXMAXMMMAMMMXASAMSSSSSMSMMASMMMSMMSMXMAAMMMAAMASAMAS
SSMMSMASMMXAXXSAMAAAMASAMASAMXMMMMMSSSSSSSSMSXSASAMSAMXAMXSXMASAMAXMAASAXXAASXMMMMXMSMSSSSMSMSMXAMAAMMSAMXAXAMAAAMMMXSMXAXXXXMSMMSSSMSAMXSXM
XMAMXMASXMAMMXMASMMMSASASAMAAAMMSAMAAXAXMAMASMMASMMMAMMAMAMMSXSAMMSMXMSSXSMMMAAXAXAAAXMMAMAAAAAXSSMSSXSMMMSMMMSSSMAXAAASMMSMSMAMXAAAXMASXMAS
MMAMAMAXAMSXSXXAXAMAMMSMMASMSMAAMAMMMMSMMAMMMAMMMMASAMSSMMSAMAMAMSAXXAXMMSAMSSMMMSMSMSMMAMSMSSMXMAXAMXMASAXAAXMAMMXMXMAMXAAAXSMMMMSMMSAXAXAS
SSMSXSXSSMAAAXMMSAMXSASASXMAAMXMSSMSAAAAMXSXSSMAASMXAMXMASMXSASAMMASMAMXASAMAAMXXXMAMAMMXMXXAMXASMMMSAMAMMSSMSMSMMMAASMSMSMSMSXSAXAXXMASMMAS
XAAAXXAAMAMMMMXXAXXMMASMMAXSXSXAAAASMMMSMSMXMXSSMMMSSMASXMAXMAMASMAMXMMMMMAMSSMMXSSMSMXSXSAMXXSASMAASMSMSAXAAMMMAAASMSAAAAMAAXAAMSASXSAAXMAS
SMMSSMSMMXSSXMMSMMXAMXMXSSMMAMMXMMMMSXAXMAXAXXMMMSMAAXASAMMMMSMMSMSMAMMXSXMXXXXMAMAMXXAAMXMAXAXXSXMXSAAAMMSMXMAMSMMMAMMMSMSMSMSMMMAMAMSSXMAS
XMAMAXMASAAAAMXAAAXSSSMXSXXMAMMMXMSAXMMMSASMSMXAAAMXSMXSAMXAAXAXXAXSASMASAMXMAMMASMMAMMSMAMSMSMXMASAMMMSMXXAXMMXAMAMXMXMAXAMXAMXMXAMXMMMMMAS
MMSSMMXMMMSSMMSMMMSMAMMMMAMSMMAXSAMXSASAXAXXAXMMMMMAMXAXAMSMMSMMMMMSXSMASAMAMAMMAMAMXMMAXAXAAAAXMAMASXAXMXMMMSXAXSMSASASXSMSMXMASXXSXXXAXMXS
MMMAXMAXMAMXMXAXAMMMMAAAMAMAMXMSMXXXMXASMMMXAMXSASAMXMMSMMXXMAXAASMMMSMMXAXSSMSMMSMMSXSASXMMMMMAAMSMMMMSMXAAAXMMXAASXSASAXXSAMXAMXSMMMSMXAMX
SAXXMSSSMMSASXXMMXXXXXXXXASASAMMMSXMXXMMSMAMMAXMASAXXSMSAAMXAMMSMSAAAXMASMMAAAXXAAMMSAMXXMASXMSSSMAXAXMAMSSMMSAMMMXSAMMMMSMXAMSAMSAXAAAMMSMM
SAMSAMAAAASXMAMMSMMSSSXSMMMAXXSAAXAMMMXMAMSASMMMMMXAAMAMMASXAXAMMXMMMXXMASMSMMMSSMSAMSMXMMAMAAAAAAXMMMSAXAMXAXAXMXAMAMXAAAAMAMSAMAMMSMMMAAAX
MAMXAMSMMMSASXMAAAXAAXMSASMSMAMMSSSMAAAMXXXMSAMXSAMXXMAMSAMXMMMSASXSSMSMMXMMAMAMMXMASAMAXMASMMMSMMSMSAXMSMMMSMXMMMXMMMSMSSSMXXMAMMXMXASMSSSM
MAMXAMXAXXSAMAMXSSMMSMASAMAAAXMXMAMXSSMSSSMXSXMSMAMMMSXXMMSAMXMMASAAMMSAXMSSSMAXXAMXSASAXXAMAMXMAASXMASXAAXSXAMMMMSMSASAMXXXXXMAMXAXMAMAXAAA
SASXAXSXMXMASXMAMMAMAAMMXMXMSAMXMAMAMAMAAAMASASXSAAAAAXSAAMMSAAMAMMMMAMAMXAAASXSSXSMSMMMSMAMXMAXMMSAMMAMSSMSMSMAAMAAMASXSXMAMSMMXSMSMSMXMSMM
SAMMSMMAAXSAMMSXXMAMMSMMAXASXXSASAMMSAMMSMMASMMAMSMMXSXMMMAAXMMMSSSMMSSMSMMMMMMAAMAAXXAASXSMSSXSAMSXMXMAXMMMAAMXMSMSMXMAMMMAAAASMSXSAAAAXXXS
SASAMASXSXMAMAAASMMXAAASMXXMAASXSXSASAMXXXMASAMAMMXSMMMXSMMMXSXXMAXMAXAAXMSMSAAMMAMSMSMMSAXAMAXAAMMSMXXXMSAMSMSMXXMAMMMAMXMMSSSMAMAMSMSMMSAX
SAMASAMAMMSSMMMSMAAMSSMAMSMMMMXMSAMXSASMSAMAXAXSMSASAAXAXAXMAXMMMAMMSMMMMXMASXXXXXXAAAMMMXMSMMMSSMAAXSASASMMAMAXMAMXMSSSMXMAAXAMAMMMAMAAXMAM
MAMMMMMSMAAMAASAMMMMAAXMAAAXAMXSMXMASXMAXXMAXSMXAMAXSSMSSMMMMXAAMMSAMAMMMMMAMMSMMMSMSMSXAMXXAMAAAMSSXSAMASXSMSAXMAMSMAAAASMMSSSSSMXMASXMAMMA
SMMAAXXAAMSXSMSASMSSSXMXSXSMSSXMASMASAMSMSMSAAMMSMSMMMAXMASAMSSMSMMASMMAMAMASXAAAAXAMAMXMSASMMMSAMXMAMXMXMAMAMAMMAMAMMSMMMAAAAXAAXSSMSAMASAM
XAXMMMMXXMXAXMMMMMAAMAMXMAAXXMASXMMSSXMAAAAAMMXAAAMAAMMMSAMAXAMMMMMXMXSAXAXMMXXSMXSAMXSAAMAAXXXMASMMXMMMMMAMAMMASMSMSAXAMSMMMSMMMMMAMSAMASAM
SSMSSSSSSXXMXAAXAMMSMMXAMXMXXMAMAXMASAXMSMSMXXMSMSSSMSAAMMSXMASAXAMMXASMSMSAAMXMXMMAXXMMXMXMMSSMASAMMAMXASMSXSMMMXAXMASXMASMXXAAMAMAMXMMASAM
AAAXAXAAMAMXMMMSSMMAMMMXMAXAMMMMMMMAXXMXAMXXXMAXAMAAASXSXMAXSXMMSMSAMXSXAASMMMASASMXMXMASAMXAAAMXSAMSSMMXSAMASXXASAMSXSAMXSMMMSASXMAXAXMAXXM
MMMMMMMMSASAXAMXXAXXSAMMSSSMAAAASMSMSSXMASMMMAAMAXXMXMAXAXAMXMMMSXAMAMXMMMXMAMXSAMXMMAMASXSMMXSMAMAMAMAMASAMXMAMXAMXMASMMASASAMXAAASMSSSSMSS
MSMSAXXXSASASXSMSSMAXASAMMAMSMSMSAAAAAASASMMXMASMMMXMXAMXSXMASAAXXMXSAAAXXASXXAMAMAAXAMAMAMXXAXMMSSMXSAMXSAMXXSMAAXAMAMXMASAXXXMMMMXAAMAAAAA
XAAXASXMMAMAMAAMAMASXMMXXXAMXAXAMMMMMSXMAXASAMAAAXSMMMSSXMASXSMXSAMAMSSSSSMMSMXSXSXMSXSXMSXMMMSAXXAMASASAMAXAAMSXMXMMSAMXAMMMSXXAAAMMMMSMMMS
MMMMAMAMMSMSMSSMXSAMXSMSMSMSMMMMMSMMXXAXAMSMSMSSSMSASAMAMSAMXXAAMAMMXAXXAAMAMMMMAAMASAMXXMASAXXXMSMMASMMASMMMXAAXAMSAMASMSSXAXMAXMSSMXMXXAMX
XAASASXMAXMXXXMAMMMSXMAAAAMAXAAAAAAMSSMMMMXAMXAMXASXMASAXMXSSMMSSMMSMMXXSMMAMAAMSMMASAMSASAMMMXAXXXMASASAMXXXMMXSMMAASAMAAXMSXMSAAMMMAMMMSSM
MSXSASAMXSMSSMMAMMASMMMMAMMAMXSMSSSMAXMAXAMAMMMSMXMMSAMXSSMMAXAMAMAAASMAMXSXSSSXMAMXXAMSAMASASXXMAXSAMXMAMXMXMMAMAASMMMMMMXMAMAMMMXAMXSAAXAX
XMMMAMXMAAXMASXSSMAMAAAXSMMSSMMMMMAMAXMXMMSAMMMXXAAXMXSMXMASAMXMAMXSAAXAMMSXAAXAXXMAXSMMAMAMASAAMSMMMSASMMSSSMMASXMMXXXXAMXMSAMXXSSSMMMMMXAM
MSAMXMAMSMMXAMXMAMXSSMMMXAMAAAMXXSAMSMSAAXSASXMAXMSMMMMXAMXMMSSMASAMMMMASASMMMMSMAMMMXASAMXSXMMMMAAAASXSAAAAAXSASMSMMMSSMSXAXSXMXAAAXMMAXMXM
ASAMSMXXAASMSSMSAMXXMASMXAMSSMMAMAMXXASMSMSAMXMSXMAAXMAXMXMAXAXXMMXSAXSAMXSAMAAAXAMXASAMXMAXAXXSXSMMXSXSMMMSMMMXSAAAAAXAAMMMMMMMMMSMMSMASMSM
AMAMMAMSSSMAMAASMMMMXMASMSMXMAMSSMMAMXMXMAXMXMAXAMSXMMSSMASXMXSAXAXXXXMXXASAMMSSSMXXAMAXAMXMMMMSAXXSAMMXXAMMMMMMMXSSMSSMMMAAAAAAXXAXAAMMSAAA
MMMMSAMXMXMAMXXXMASXASAMXXMMXSMMAXMXSASAMMMMXMAMAMMXMAXAMASAMASMMSSMSSSSMASXMAXMAMSMSSMXMSMMSAMMAMAMAMAMSAMAAAAAMXMAXMAMXSSSMSSMXSASMMSAMXMM
SASMSASXSAXXSXMASXSXMMXXXAXSAMASMMXAXAMMXXAXXMSSSMXAXASMMMMAMASXAMAASAASMXMAMXSMAMXAAXMASAMAXASMXMMSMMSXMASMSSSSXSXSMMMMMMAXAXAMAMXAAMMMSMMM
SMSAMASASMSMMMSAMXMASMSMMMAMASXMMAMSMSMSMXSSXSAAMASXMAAXAMSSMASMMSMMMMMMMMMXMASMSSMSMMSXXAMASXMXAXXXASMAMAMMAMXMASAMXXAAMAAAMSAMXSMXSMAXAAAX
XXMAMXMAMSAAAAMASASMMAAAASXSXMMAMAXXAAAAAAXAMAMXMXXMASMSSMAMMMXAMMXXAMXAAASAMXSAAAAMAAAMSXMASAASMXXSMMMAMASMMMXMAMAMMXSSMMXSXAXSAXXAMMSSMSMS
MSSMMSMMMXXXMSSMSASAMSMXMXAMXXAXSMSMSMSMSMXASXXAXMAXMAXXAMXSXASMMMMMMXSSSSSSSXMMMXXSMMSAAAMXMMMMXSMXMASASXSASAXMMSSMAMMMMSAMXSSXXXMASAXMAMAS
AAAMAMASMSSMMAMAMAMXMXMXSMSMSMMMXMAAXXXAAXSXMMSXSXXMSMMSXMMXMXXMASAASMAAMAMAMMMXSXMAXMAMSSMSXAXSAAAXMASASASAMMXAAAMMXSXAAMASAMAMMXMSMXMMSMAM
SMMMXSMMMAAAXAMMMMMMMAMAXAXAXAMSASMSMSSSMMXAAXSMMMSMAAXMASASMSMSAMSSMAMAMAMAMSMAMXAXXXMAXAAMXAMMXMMXMMMXMXMXMASMMMSAASXMMSSMAMMAXAXXXAMXXXXA
AMAMXAMAMSSMMXMAAAAAXAMMMSMMMSMSAMAXAAXAXXSSMMMAAAAXMSASAMMAAAMMSMXXMSXASASAMAMASASMMMXSSMMASXXMMSMXMXMXAMXXXXAXSAMMXXAXAXXSXMASXMSMSMMMMMMA
MSASXMSAMMAXAMSSSSSSSXSXAAAMXXXMAMAMMSMXXXAAMSSSMSSSSXMMMSSMMMSAMXAXMASXSAMMSXSAXXXAMXAAXAMAMAAXAAAAMASMASXSMMSXMASMSSSMMSXMXSMAAAAAAMAAAAAS
XSASXXSXMSAMXXAMAAAAMMSMMSSMMMMSAMXXAAAASMSAMXAXMXMXMAXMMAAXXXMMMMMMSAMXMMMAAAMMSMSSMMSMSXMASXMMSSSXSAXSAMXMAMMMSAMXAAMAXSAMASXAMSMSMSSXSSSM
XMASMXMAMMASXMASMMMXMAXAXAAAAXAXXXSXSMSMXAAMXMMMSAMASXMSMSSSSSMXAMXAXAXXAAMSMXMAXMAMAXAASMSXSAMAXMAMMXXMXSXXAMAAMASMMMMSMXAMASAMXAAMMAMAAMAM
XMAXMAMSMSXMASXMXXAAMMSMMMSSMASMAMSMXXMAMMMSSXSASASAMAAAAAAAAAAASMMMSMSMSMXMXXAMXMSSSMMSMAMMMAMSMMSMSMMMMMMASMMXMMMMXSXXMSXMAXXSSMXMMASMMMAM
AMASXMXAAAAXMAMSAMXMXXAXAMAAMAXXMMSAMSMMSAAXMAMASMMASMSMMMMMMMMMMAAAXXAAAXXMASMXMMMAMXXAMAMMSMMAMSAAAAAAAAAMXASMSMAAAMASAAAMMXAMXMASMMSAXMAS
XMASASXMSMSSSMXSXMASMSMSSMXXMXSAMAXMAMXAMMSSMXMAMASXMXXXXXXMAASXSSMMSSMSMSSMAXMAMAMAMMSMSXMXASXXSXMSMSMSSSSXSAMAASMMXSAMXMMXXAMXXMASAMMMXSAM
XMMSMMAAAXXAAMXMASMSAAXAXXAMSMSXMAXSSMMMSAXAMMMSMMMMSMMMSMMSSSSMAAAAMAAMAMMMSSXSSXSXSAAMAMXMASXXMAMMAXAAAMXMAMMXMXXAXMMMMXMAMSXMSMAXASAMXMSS
SMXSXSMMMMMSMMASXMAMMMSSMMXSAAXMASMMAAAAMASAMMAMAXXMAAAAXAAAMXMAXSMSSMMMAMAMXAAXAMXMMMMMSASMAMASXSMMAMMMXMMMMMSMXMMMSMAAXAMAMAAXXMAMXXAXXAAS
SXAMMXAASXAXXSAXAMXMAMAXAMXXMXMMSMASMMMSSXSAXMASMMSXMMMXSMMSSSSSMMMAXMAMASASMMMMAMMXMASXMAMXAMXMXAMMSSXMXMASAAAAAAAMXXSMSMSXSSSMMMMMSSSMMXMS
MMMMAMSMMMXSAMXSMMMXXMASXMMMSMAXMXAMXXMAMAMXMMAXAAMMSSSXXASMXAAMAXSMXXMXMSAXAXXXMSMXSASAMAMSXSXAXASAAMAAXXAMMSMSMSSMAMMASMSXAAAMSAAAXAMXAAAX
SXMASMXASAAMAMMMAAXMSXMAXAAAAXMMSMSXSXMASMSXSMSSMMXAAAMAMSMMMMMMMMMXSASAMMMMSSMAMAAXMXXASXMMAXMAMXMMSXSMMMMSXMAXXMAMXSMAMASMMMMMSMSSMAMXSMMM
AXSAXASAXMXMSMASMXMAXAXAXXMSSXSAXAAASMMMSXMMMAXMASMMMMMSMXAMXMXSAMSASAXMMAMXMAXSMMSMMAXMMMAMAMMSSXSSXXMAXAAMASAMASAMXMMXMAMAXXXMXAAXXXSAXAAA
MMMAMXMXMMXXAMASXAASXMMMSMMXMASXMMMXMAAXMAXAMAMMAMXXAMXAASAMAMMSASMAMMMSSMSSSMMXASMMXMASAMAMMSAAAAXASXMSMSMSAMXSAMAAMSSMMXSMMMMMMMMSMXMASMMS
XXMAMASXSAASXMASMXXMAMAAAMMSMMMXSXMXSSMSASXSSSMSASMMSXSMAMMSAXXMXMMXMAAAAAAAAXASMMAXMXAXASXXAMMSMSMMMXAAAAAMXMAMAMAMXAAXSAMXXAAXXSMXMAMXMAMM
SMSMSMSAMXMSAMXSAMMSMXXMXSAAAAXMASXAXAXMXXXXAAXAASAAMAMXMAMSASAMSXXSSMSSMMMSMMMSXMAMMMMMXAXMASAXXMAAAMSMMMSMSMXSSMMXMSSMMASMMSXSAMXMSXMASAMS
AXAMXAMAMXAMAMXMMSAAMAMSMMMSSMMAMXMXSSMSAMMMSMSMMMMAMAMSMMXSAAMAAASAMXAMAMXAMXASAMAAMAMXMAAMXMASMSSMMXMAXAXAXXMAMAMXMAXASAMXAAXMAMAAAXMXXXSA
MXMSMMMAMXMXSMSAAMXMMASAAMAXAMXMXAAXMAAMXXAAAXAXXAXAMAMAAMASAMMXXXAXXMXMMMMXAMXMAMASXXSXMXMASMMSAAXXMASMMSMSMSAXMSMMMASMMASMSSMXAMXMASMAXXXM
XAXAMXXAXAXXXAMMMSSMXXSXSSXSAMSSSMSXSMMMXXXSAAMMSASMSSSMSMAXAXMSSSMXMSSMAASMSMSSMMMMAXSXSAXXAAAMAMSAMXMXAMXXASMSMXAAXXSAMXMXAAXSSSMXAAMMSMAA
SMSASMSMSASMMXMAMSXSAAMAMAAMAMAMAAXMASMSSXMAMAXAXAAAAMMMXMMXSMSAAAXAMAMSMMXAAAAASMMMMMXASMSMSMMMXAMXMAMXSXMMXMASAMSMSMXMMSSMMMMXMAMMSXXAAMMM
AASAMAAXXXMMAXSSXMASMSMSMSMSAMMSMMMAMAMAMMMSSSSMSAMMMSMMMASAAXMMXMXXMAXSMSMSMMMSMSAAMMMMMAXXAMASXXXAMASXMAXSXMSMMMXAMXXAAAAMASXMSAMXMMMSSMSX
MAMAMMMSMAMXMMXMAMAMXAAXMAXMAMXXXAAAMAMXMAAMAAAAXAXMAMAASAMSSMSXSMSMSMSMAXAXSSXMASXMAAMAMXMMASASAMSMSASASMMMAMXAXAMAMASMMSAMXSAAMMMAXMAMAASX
XASAMXAXXXMAMXASXMSSXMSMMMMXSMMSSMSXSMSMXMXSMSMMSMMMASMMMAMXXAXAXAAAAMAMXMXMASAMAMMMSASASAMMAMXSAMXMAXSMMMAMMMSMMMSAMXSXAXAMXSMMMMSASMSXMXMA
SASXSMXXMXSMSMMMMMAAXSAMXXAAMAMXAAMAXMAXAAMSAMXXAXAMAMAASXMMMMMMMSMSMSSSMAAMXMXMXMAAAXSASASAMXASAMASXMMXMXAMAAXXAXXAMMXMMSMMMMMMAMAAMAMASMMM
XAMXMXSAMAMAMAAAAMMSMSASMMMMSAMSMMMAMSASMSMSASMSMMMSSSSMMXMAMAAXMMMMAAAMAXXXSMMMMMMSXMMAMAMXXMASAMXMASAAMMSMMMSMSSSXMMXSAAAASAAXAMMSMXMASAAX
MSMSAASAMMSASXSSXSXMAXAMXAXASAMMASXXAMASXMAMAMAXSAMXAAAMXXSASMMSASXMMMSMSMSXAAMXAMXMAAMXMAMXMMAMMMASXMSXSAMAMMAMMMMAAXAMSSSMSSMSASXAMAMASMMS
XAAMMXMXMXMAXAAMMSMMMMSMMSMMSAMSAMMMAMXMASXMMMSMSSSMMMMSSXMASAASAMASXAXAAAASMMMSMSAXMMMSSXSAMMSSMSXSAMAXMAMAMSMSAAMXMMXXAMAAXAXMXMXSMMMAMAXM
SMXMXMSMMXMSMMMSAXXAAMAAAXAXSXMMXSASXMMSAMASXAAAMXMAMAXAXAMSMMMMAMXMMMSSMSMXMAMAMMMSMAAXMASASAMAAMMSXMASMXXAMAMSMXSASAXXMSXMMSMSXMAXAMMMSMMM
AMASAMAMXMAMXAAMASMSSSSSMMAMMMMMMMXSAAAMASAMMXMXMAXAMXMMSXMASXXXAMXSXXAMXXAASMMASAMSMMSSMAMMMMSSMSXSMSAMAASXMMMXAASAMSMMXMXMAAAAAMASMMAMAAAM
MMSSXSASXMSMSXSMAMMMAAAAXMMAMXAAAMASMMMSAMASAXSASXSMSMAXMASASMSSSMXSMMSSSMSXMXXXMAXXXXMMMSSXAXMASAMMASMMSMSAAXMMSMMXMAMMAMMMSSSSSMXMMSXSXSSS
XSAMXMXSXAAASXMMMSMSSMSMMXSAMSSXSMASASAMXSAMXAXMMXSAAMAMSAMXXAAMAMAXXAAMAAMXMSMSMSSMMMSAAAXMASMXMMAMAMAXMAXMMMMMMAXMSSMSAXSAAAXAXXAAMMMMMMMM
MAAMSMMMMSMXMAXXAAAXAAAXAXXAMAMAXMMSAMXSAMXSSMSXSAMSMMSAXXMMXSXSAMSSXSXSMMMAXAAAAAMAAASMSXXXMXXSMXSMMSMMMSMMXAASMXMASAASASMMSSMMMSSSMAAAAAAM
SSXMAAXMAXXASMMMSMSSMMMXSMSSMAXMMSXMASAMXSMMAXAXMAMMXMXMSMMSAAMSASAMXMMXXMXXXMSMSMSMMMSXMMSSMSAMAAMAXAAMAAAXMSXMASAMXMMMXMAXAMMXAMMAXSSSSSSS
AASXSSMMMSSMMAAAXXMAMXXXMMAMSMAAMXAMAMASMXASMMXMSSMMXMAXXAAXMMMMMMXMASASMMMSAAXXMASMMXMAMAAASMAMXSSMSSSMSSSMXMMSSMSAMXMXMMXMSSMSMMSAMMAMAAXX
MMMXAMASMMAMSSMSMXSAMXXXAMAXAXMMSXSMASAXMAXAMSSMMMASXSMSSMMSMSMSXSASASMMAAAXXMAXSASAXAMAMMMSAMAMXMAXAAMMXAMMMAAMAAXXAASAXXXAXAXXAAMXSMMMXMAM
MSMMXXAMMXAMMMAAAASAMMXSMSXSAXSASAXSMMMSXXSXMASAASMMASAAXMAAXAAAASAMXSMSMMXSAMXMMAMXXMMSXXXMMMMXXMAMMXMXMXMAXMMSMMMSSXSASMMMSMMMMMSASAMMAMXM
MAAMSMSSXSASAMXMMMSAMSMMAAXMASMAMMMXSAXMAMMXMASMMSAMXMMMMMSSSMSMMMXXMMXAAAASMMMMMMMSXMAXMAXXXASXSAXSMXAXSXSASXMSAMAMAAMMMAAAAAAAAMMMSAMSMMAM
SMSMAAAAASXMASAXXAMAXMAMMMMXXAMXMSMAMMXAMXAMMXMAXSMMSMAMSMAMAXXXXAXSXMSSSMXMMAXMAMAXAMASAMMXSXSASAMAXAMXMXMAMAAXSMASMAMASMMMSMMXMXAAXMMAMSAS
MAAXMMMSMMXSMSASMSMMMSSMSSMSXSAMXAMASMSMSXSAAMMSMMMAXSASAMASAMAMMSMMAMAAXMMMSMXSASMSSMASMMMXXXMMMMSXSXMAMMXMSMMMMMMXXMAMMAMAMAAASMXMXAXAXSAM
MSMSXAXXAMXSAMXXXXASXXXAMAASXMAXSASASAAAXAAXAAAAAXMSMMXSASXMMMSXAAASAMMSMAAXAMMSAMMAAMASAASXXMAXAXSXMASMSMAMAXSXMASXMASMXSMAMMMMSAAMSSMSMMAM
XAXMXMSSMMMMMMMMMSXMAMMXMMMMASAMMASMMXMMMMMMSMSSSMMXAMASMMMXSAXMXMMAMMAMXSMSASAMSSMMMMXSXMXXMASXMSMMSXAAAMSSXSAXSAMAMSMSAMMMSMMXXMMMAAXXASAM
XMSMAMAMXAAAAAAAMMMSAMAXSAMXMMMMSAMXAAMSSXSXXAAMAAASAMXSAXXAMASMMSXMSSSMXMASAMXSASASMMXMMSSMSAMXMAMAXMMSMMMAMMMMMASXMAAMXXAAAASMMMSMSXMMAMXM
MSAMAXASXSMSSSSXXAASXXXSAMXSAMXAMASMSMSAMXXMSMMXSMMMAMASXMMXMAMAXMAXAAAXMMMMAMXMASXMAMXMAMAXMXMAXAMMMAXAMAMAMMXMSAMASMSMSSMMSXMAMAAMXASMSSMM
XMAXMSMSAMXXAMXMMSXMXAMXAMAMAMMMXAMXMMMMSMSMXAAXXAMSSMMMAXXAMXSMMMMMMSMMXAAXAMXAMXMSAMAMSSMMMMSXSASXSSSMXAXASXAAAXSXMAAAAXMXXXSXMSSSXMMAAAAM
MXSXAAAXMXMSAMAMMXAMSSSMMMXXAMXSMSSMXAAXAAAMMMMXSAMAMASXMMSMSAMXSAAAXAXMSSSSXSXXMAXMAXAXMAXMAAAMXAAMXAAASXSMSMSSXXMAMXMMMSMXMASXAMAMAXMMMSSM
SAMMSMSMMAMXAMXMAMAMAMAAMSXSXSAAAAAASXSSSSXMAASXSMMXSAMASXMAMXSAMMXMMMXMAAAAASMASXSSMXSMSAMXMSSMMMAAMSMMMAAAXAAMMMSSMSXSMAMXMAMMAMASMMSAAMAA
MASAAAAMMASXSMSMXSAMASXMMSAAASAMXMMMAAMAAAASXMMMSAMAMMSMMAMSMAMMSSMMMXSAMMMMXMAXAAAMAAXXMXSXAAAXMAAMMAMAXSMMMSMMAAAXAAAMMAMXMASAXMASXAMMSSXM
MAMXMSMXXASAXAAMMMASASAXAMAMMMXXMMXMMMMSMMMMSAAASAMXMAAAMAMXMAXMAAAAAAAMAXXXAMSMMSMMMMSAMASMMSXMMSMXMASMMMXXAAMXSSMMSMAMSMSMSASMXMAMMMSMAMMM
SSSSXMXMASMXMSMSASAMASMMAMXMASMAMXAMXXAAXXXAXSMMSXMASXSSSSXXMAXMXMMMMMSXAXXSXSAAXAAAXMMAMAXAXXMMMAMASAMMAXSMSXSAAAXAMMXMAMAAMAXMXMASXMAMAMAX
SAAAAMSAMXAXMXXSXMMSMMMSMSMMASASMSSSMMMSSXMAMMSMXMXMAXAXAMXSMMSMSASXAXAMXMXAXSXXMSSMSSSMMSSSMMAXSASAMASXSMMXMAMMSMMXMSASASMSMASXMSASMSSSSSSS
MMMMMMXMAXMXAAMMMMAAXAAAASASAMXAAXAAXASAMXMMMMASXASMSMSMSMAMAMAASAMXXAMXAAXMMMXXMAMXAAXSAMXMASMMMXMAMAMXAAXAXMMAAXXAXXASXSAXMASMAMXSAMAAAAAA
MMXMASXSSMSMMMSAAMSMSMMSASAMASMMXMSMMSMASMMSASAMMMXAMAMAMMXSAMMXMAMASXXSXSMXAASMMAXMXMSMMMAXXMXAXMSSMAXASMMMSXXSASMMSMMMXMXMMAMMAMMSMMMMMMMM
MASAMSAAASMAAAMMSXXASXAXMMXMMAMXSAAMMMXAMAAMMMAXAXMXMAMAMAMSASXXSAMAMSAMXMASMMMASAMSAMAAMSSSMMXSAAAMSMMMXSASXMAXMMMAAAASXSXAAAXSASAMXXSASAXX
SASMXMMMMASXMSSMMAMAMXXXAMXSMAMAMMXSASMMMMSXMXSMSSSMSXMAMAMSAMXAXXMASXMASMXMSASMMMXSASMMMXMAMAAAMMMMAMXMASXMXMAMAAMMSSMSAMMAMXMXAMXSAASASMSA
MASXXMXAXMMXAAAMXAMMMMSSXMASMXMASMMMMSAXSAMXSAAMAMAAXMSMSMXMMMMMAMMMSMMASXAXSMMAAXAXAMXSAXSAMMMSXMMSMSXMAMASXXXXMMSAAAAMMMMASMSMSMSMXXMAMAXA
MMMMMMMMMXSMMSSMXXXAXAAAAMAXMAMXMAAAASXMMASAMSMMASMMMMAMAMAAAAAAAAAAMAMASMMMMXSSMMMSMSMAAMMMSMAMASXAXAAMXSAMMSMSMAMMSMMMAASXMASAMXXMMMMMMMMM
SMAXAAAXXAXAMAXASXSMSSMMXMASXXSAMSXMMMXXMAMAMAMSMMXAAMASXSSXSMSXMMMMSXMAMXMAXAAMAXXAAAMMSMXAAMMSAMXMMSXMXMMAMXAAMXMAMXXMSASAMSMMMSSMAMASAMAS
MXMXSXSXMXSXMASAMAAAAXXXAXAXAAXMMAXSAMMMMASMMMMAAMSXXXASAXMAMAMXSASXMAMMMSXMSSSSSMSMSMSXAXMXSSXMAMASAMMSMXSMSASMSXMXSMSXXAMMMAAMAAAXMSASASAS
SASAMMXASASAAXMAMSMMMSSSXSMMMMMSMSMXAAAXMASAAMSMSAMASMMMMMMXSAMASASMSAMXASAMXAAAMAXAXXAXXMMSMXXSXMMMASAAAAMASAMASMSMAAMAMXMASXSMMXSMAMASAMAS
SXMASASAMASAMXSXMMAXSAAXMXMASAXAAXASXSSXMMMXMMXMMAMAMAAAAAMAMAMXMAMAXSXMASAMMMMMMSMMMSMSSMSAAXMASXXMAMXMSXMXSAMXMAAXMMMAMASXSXMASXMAMMMMAMSM
XMASMAMAMXMAXASAMSSMMMSMSASXSMSSSMAMAAAMSSSSMXXXSXMAMMSMSXSMSMMMMMMSMMXSASAMAAXAAXAMASXAAXAMSMXXAMXMASXMAXMASMMXMSMSMMSASXXSXASMXASASXSMMMAM
SAMXMMMMMMSSMASAMAAXSXXXSXXAXMAMAMMMMMMMXAAAXXSAMXSSSMMXMXSXMASXMAAAAXMAASAMSXSMMXSMASMSSMXSAAMXMXMMAMXMAAMAMAAXXAXAAASAXXMAXAXMSMSAMXSAASXS
AXSXMAAAAXAXMASMMSAMXAXAMMMSMMSMSMSASMMSMMMMAMMSMXXMAXXASAMAXAMASMMSSMXMASXXMAMASAXMXSAAMAXMMSMSMASMMXXMSXMSSSMXSSSMSMMMAMXMMMAAXXMAMASXMMAM
MMMASMSSSMSSMASXXXXMAXMAMAAAAAAAMASASXASAMAMASAMXXASAMSMMASAMXSMMSXAXAMXMSMSXMSAMXSSSMXMXSAMXMAMMAMAAASXXXXMXAXMAAAAMXXAAMAXAXMSMMSMMASASMAM
XASXMXAMXAMXMSMMMMAMAMSXMMSSMMMSMAMMMMMSASXSMXAMMMMMAMAMMMMMSXAXAMMMMSXXXMAMAMMMMAXMAMAXMMMASMMMSMSXMMSAMXSMSMMSMSMMSSSSXSXSSXAAMXASMXSAMSSS
SASXXMSMMSMXMAAAXMAMAMMASMXMMSAAMAMXXAXXASAXXSMMASXSXSMSXMAAXMAMXMAAAXMMXMXMAMAAMMMXAMSMAAXMMAMAAXMASXMAMAMMMXAMXMAMXAAAAXMAXMSMMSMSXAMXMAMM
SAMXAAAAAXMASXSMSSXSXMSAMXSAASMMMAMMMSMMMMMMXAXSAMAMXSXMASXSMSMSASMSMSAMAMASMSXMAASMMMAASMSXSAMMMXXASASXMASASMMSAMSSMMMMXMASXMAMMMMXMASAMMSM
XAMMMSSMMMMAMMAAXAASAMMSSMMMMXAXSXSAAAXAAAAXXMXMASMMAMAMMMXAAAAMAAAMXSXSASXSXMAXMMMASXMMAMXMSMSMXAMXSAMXSASASAAMAMAAASXXAASMMMAXAAMAMXSXSAMX
SAMXAMAMSSMMSXMAMMMMAMAMMXAXMSSMMSSMSSSSSSSSMXMXAAAMASAMXXSMMMSMSMAMXMASASMXAXAASXSMMAMMAXMMSXAAMSMMMXMAMMSMMMMSMMSSMMSSXMXAMSXSSSMAXASAMXSX
MMXMASXMAAAAAAMAMXASMMXSASMSMAXAXMXXXMAMAXAXMASMMSXXAXAXAAAAAXXAXAMXAMAMMMMSMMSXMASASAMSMMMASMMXMAAXMAMXSASAXXAAAMAMXMXMSXMAXAXMAMXXMASXSAAM
XXXSXMXMSSMMSMSASXXXXXSMMMMMMASXMMMMXMAMXMMMSXSXXMMMSSSMAXXSMSMMMASMMSXSXAAAAAMAMAMMMAMASMMASXSASMSMSAMAMAMMMMSSSMMXAAAXAXSXMSMSAMXSSMMMMAXA
MSMMAAAXMXAAAASMMMSMXMASAAASAASAASAAMSASMASAMAMXAAMMMAMMMSAAXSAAMXMAMMAMMMXXMMSAMASXSXMMMMMMXASXSAXXSASXMSMSAXAXAAMSSSSSMXSXXXAMXMXXMASASMSS
AXASMMMMMSMMMXMMSAXMASMSSSSSMXSMSSMXXAAMXAMASAMSSXMAMSMAAMMMMSMMSSSMMSAMASAXSXMXXAMXSAMXASXAMMMMMAMAMMXAAAASMSMSMMMAAMAAXASMSMAMMMSASAMXAAAX
MSXMXMXAXAAAXMAMASXMAMXMAXAXAXMMAXXSAMMMMMSASMXAMASXXAMMMSASMXASAAAAASAXAXMXMAXASXXMXMXMMMXAXMAMMAMAMASMMMXMAAXAXXMMMMSMMMSAAMAMXXAAMMSAMMMM
MXMAASASMSSMXXMMAMMSSMAMXMMSXMAMXSMMAXAMAAMMXXMAMMMSSMSXASXSAMXMASMMMSAMSSSMSMMMXAASXMSSSMSSSSMMSXSXSMSSMSSMSMSSSMASXAXSMASXMSSSSMMSMXXMXMMM
AXASXMAXXXXMASMMMSAAASMMMAMAXMXMAXXSAMMSMSSMXSSSMSAAMXMMMMXXMSMSXMASAMAXAAAAAXAXMSMMAMAXAAMAAMMAMASAMXSAMAAMAMXAAXAMMSMAXAXSXXAAXXXAASMMASAM
MAXXSMMMMSMMASXAAMMSXMSAAXMAXMAMSSMMASXAXXAMAAAAMMMMSAMASXMSAXAAAAXMXSMMMSMMMSASMMMSAMASMMMMMMMAMXMSAMXMMSMMASMSMMMSAAMMMMMMXXMASMAASMASASAS
XMSAMXAAMAXMXSXMMSAMXASXSXMAXSAMXAASAMXAMSAMXMSMMAAMSASASAAXMMSMMAMSAAAXMAMSAMXAAAASASASAAXXXXSASAAXMSAXMAASASAMAXMMXSSXXMASAXXAMXAMXXXMXSMM
AXMXMSSSSMSMXMASAMXSMMMXAXMAMSASXSMMMSAXMSXXAXMASXSXSMMMSMSMXAMXSXAMSSXMSXSSXXSSMMMSAMXSXMSAAXSASMSMXSASMSXMASMSSMXMAMMAMXASASXMASAXSAMXXMAS";
}
